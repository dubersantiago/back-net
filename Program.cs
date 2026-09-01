using System.Text;
using back_net.Constants;
using back_net.Repository;
using back_net.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var dbConectionString = builder.Configuration.GetConnectionString("ConexionSql");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(dbConectionString));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var secret_key = builder.Configuration.GetValue<String>("ApiSettings:SecretKey");

if(String.IsNullOrEmpty(secret_key)) throw new InvalidOperationException("Secret key no esta configurada");

builder.Services.AddAuthentication(Options=>
{
  Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(Options =>
{
    Options.RequireHttpsMetadata = false;
    Options.SaveToken= true;
    Options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key)),
        ValidateIssuer = false,
        ValidateAudience = true
    };
});

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(Options =>
    {
        Options.AddPolicy(PolicyNames.AllowSpecificOrigin,
        builder => builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
        );
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors(PolicyNames.AllowSpecificOrigin);
app.MapControllers();

app.Run();