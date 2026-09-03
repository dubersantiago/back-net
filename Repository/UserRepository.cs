
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using back_net.Models;
using back_net.Models.Dtos;
using back_net.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace back_net.Repository;
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;
    private string? SecretKey;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _rolManager;
    private readonly IMapper _mapper;

    public UserRepository(ApplicationDbContext db,IConfiguration configuration, UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> rolManager,IMapper mapper)
    {
        _userManager=userManager;
        _rolManager=rolManager;
        _mapper=mapper;
        _db=db;
        SecretKey=configuration.GetValue<String>("ApiSettings:SecretKey");
    }
    public User? GetUser(int id)
    {
        return _db.users.FirstOrDefault(u=>u.Id==id);
    }

    public ICollection<User> GetUsers()
    {
        return _db.users.OrderBy(u=>u.Username).ToList();
    }

    public bool IsUniqueUser(string name)
    {
        return !_db.users.Any(u=>u.Username.ToLower().Trim()== name.ToLower().Trim());
    }

    public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
    {
        if (String.IsNullOrEmpty(userLoginDto.Username))
        {
            return new UserLoginResponseDto()
            {
                Token="",
                User = null,
                Message = "El Username Es requerido"
            };
        }
        var user = await _db.applicationUsers.FirstOrDefaultAsync<ApplicationUser>(u=>u.UserName!.ToLower().Trim() == userLoginDto.Username.ToLower().Trim());
        if(user==null)
        {
            return new UserLoginResponseDto()
            {
                Token="",
                User = null,
                Message = "El Username No encontrado"
            };
        }
        if(userLoginDto.Password == null)
        {
            return new UserLoginResponseDto()
            {
                Token="",
                User = null,
                Message = "El Password Es requerido"
            };
        }
        bool isValida = await _userManager.CheckPasswordAsync(user,userLoginDto.Password);
        if (!isValida)
        {
            return new UserLoginResponseDto()
            {
                Token="",
                User = null,
                Message = "Las Credenciales son incorrectas"
            };
        }
        var handlerToken = new JwtSecurityTokenHandler();
        if (String.IsNullOrWhiteSpace(SecretKey))
        {
            throw new InvalidOperationException("Secret Key no esta configurada");
        }
        var roles = await _userManager.GetRolesAsync(user);
        var key = Encoding.UTF8.GetBytes(SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id",user.Id.ToString()),
                new Claim("username",user.UserName ?? String.Empty),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? String.Empty)
            }
            ),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
        };
        var Token = handlerToken.CreateToken(tokenDescriptor);
        return new UserLoginResponseDto()
        {
            Token=handlerToken.WriteToken(Token),
            User = _mapper.Map<UserDataDto>(user),
            Message = "Usuario Logueado correctamente"
        };
    }

    public async Task<User> Register(CreateUserDto createUserDto)
    {
        var encriptedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        var user= new User()
        {
            Username=createUserDto.Username ?? "No Username",
            Name=createUserDto.Name,
            Rol=createUserDto.Role,
            Password=encriptedPassword
        };

        _db.users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
}