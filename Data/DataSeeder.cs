using back_net.Models;
using Microsoft.AspNetCore.Identity;

public static class DataSeeder
{
    public static void SeddData(ApplicationDbContext appContext)
    {
        if (!appContext.Roles.Any())
    {
      appContext.Roles.AddRange(
        new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
        new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
      );
    }
    // Seeding de Categorías
    if (!appContext.Categories.Any())
    {
      appContext.Categories.AddRange(
        new Category { name = "Ropa y accesorios", creationDate = DateTime.Now },
        new Category { name = "Electrónicos", creationDate = DateTime.Now },
        new Category { name = "Deportes", creationDate = DateTime.Now },
        new Category { name = "Hogar", creationDate = DateTime.Now },
        new Category { name = "Libros", creationDate = DateTime.Now }
      );
    }
    // Seeding de Usuario Administrador
    if (!appContext.applicationUsers.Any())
    {
      var hasher = new PasswordHasher<ApplicationUser>();
      var adminUser = new ApplicationUser
      {
        Id = "admin-001",
        UserName = "admin@admin.com",
        NormalizedUserName = "ADMIN@ADMIN.COM",
        Email = "admin@admin.com",
        NormalizedEmail = "ADMIN@ADMIN.COM",
        EmailConfirmed = true,
        name = "Administrador"
      };
      adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");

      var regularUser = new ApplicationUser
      {
        Id = "user-001",
        UserName = "user@user.com",
        NormalizedUserName = "USER@USER.COM",
        Email = "user@user.com",
        NormalizedEmail = "USER@USER.COM",
        EmailConfirmed = true,
        name = "Usuario Regular"
      };
      regularUser.PasswordHash = hasher.HashPassword(regularUser, "User123!");

      appContext.applicationUsers.AddRange(adminUser, regularUser);
    }
    // Seeding de UserRoles
    if (!appContext.UserRoles.Any())
    {
      appContext.UserRoles.AddRange(
        new IdentityUserRole<string> { UserId = "admin-001", RoleId = "1" }, // Admin
        new IdentityUserRole<string> { UserId = "user-001", RoleId = "2" }   // User
      );
    }

    // Seeding de Productos
    if (!appContext.products.Any())
    {
      appContext.products.AddRange(
        new Product
        {
          Name = "Camiseta Básica",
          description = "Camiseta de algodón 100%",
          price = 25.99m,
          SKU = "PROD-001-CAM-M",
          Stock = 50,
          CategoryId = 1,
          Category = appContext.Categories.Find(1)!,
          imgUrl = "https://via.placeholder.com/300x300/FF0000/FFFFFF?text=Camiseta",
          CreationDate = DateTime.Now
        },
        new Product
        {
          Name = "Smartphone Galaxy",
          description = "Teléfono inteligente con 128GB",
          price = 599.99m,
          SKU = "PROD-002-PHO-BLK",
          Stock = 25,
          CategoryId = 2,
          Category = appContext.Categories.Find(2)!,
          imgUrl = "https://via.placeholder.com/300x300/0000FF/FFFFFF?text=Smartphone",
          CreationDate = DateTime.Now
        },
        new Product
        {
          Name = "Pelota de Fútbol",
          description = "Pelota oficial FIFA",
          price = 45.00m,
          SKU = "PROD-003-BAL-WHT",
          Stock = 30,
          CategoryId = 3,
          Category = appContext.Categories.Find(3)!,
          imgUrl = "https://via.placeholder.com/300x300/00FF00/FFFFFF?text=Pelota",
          CreationDate = DateTime.Now
        },
        new Product
        {
          Name = "Lámpara de Mesa",
          description = "Lámpara LED regulable",
          price = 89.99m,
          SKU = "PROD-004-LAM-WHT",
          Stock = 15,
          CategoryId = 4,
          Category = appContext.Categories.Find(4)!,
          imgUrl = "https://via.placeholder.com/300x300/FFFF00/000000?text=Lampara",
          CreationDate = DateTime.Now
        },
        new Product
        {
          Name = "El Quijote",
          description = "Novela clásica de Cervantes",
          price = 19.99m,
          SKU = "PROD-005-LIB-ESP",
          Stock = 100,
          CategoryId = 5,
          Category = appContext.Categories.Find(5)!,
          imgUrl = "https://via.placeholder.com/300x300/800080/FFFFFF?text=Libro",
          CreationDate = DateTime.Now
        },
        new Product
        {
          Name = "Jeans Clásicos",
          description = "Pantalones vaqueros azules",
          price = 79.99m,
          SKU = "PROD-006-PAN-BLU",
          Stock = 40,
          CategoryId = 1,
          Category = appContext.Categories.Find(1)!,
          imgUrl = "https://via.placeholder.com/300x300/4169E1/FFFFFF?text=Jeans",
          CreationDate = DateTime.Now
        },
        new Product
        {
          Name = "Tablet Pro",
          description = "Tablet 10.5 pulgadas con stylus incluido",
          price = 459.99m,
          SKU = "PROD-007-TAB-SIL",
          Stock = 20,
          CategoryId = 2,
          Category = appContext.Categories.Find(2)!,
          imgUrl = "https://via.placeholder.com/300x300/C0C0C0/000000?text=Tablet",
          CreationDate = DateTime.Now
        },
        new Product
        {
          Name = "Zapatillas Running",
          description = "Zapatillas deportivas para correr",
          price = 129.99m,
          SKU = "PROD-008-ZAP-BLK",
          Stock = 35,
          CategoryId = 3,
          Category = appContext.Categories.Find(3)!,
          imgUrl = "https://via.placeholder.com/300x300/000000/FFFFFF?text=Zapatillas",
          CreationDate = DateTime.Now
        },
        new Product
        {
          Name = "Cafetera Express",
          description = "Cafetera automática con molinillo integrado",
          price = 299.99m,
          SKU = "PROD-009-CAF-BLK",
          Stock = 12,
          CategoryId = 4,
          Category = appContext.Categories.Find(4)!,
          imgUrl = "https://via.placeholder.com/300x300/2F4F4F/FFFFFF?text=Cafetera",
          CreationDate = DateTime.Now
        },
        new Product
        {
          Name = "Programación en C#",
          description = "Guía completa de programación en C# y .NET",
          price = 49.99m,
          SKU = "PROD-010-LIB-ESP",
          Stock = 80,
          CategoryId = 5,
          Category = appContext.Categories.Find(5)!,
          imgUrl = "https://via.placeholder.com/300x300/008B8B/FFFFFF?text=C%23+Book",
          CreationDate = DateTime.Now
        },
        new Product
        {
          Name = "Chaqueta Deportiva",
          description = "Chaqueta impermeable para actividades al aire libre",
          price = 149.99m,
          SKU = "PROD-011-CHA-NAV",
          Stock = 28,
          CategoryId = 1,
          Category = appContext.Categories.Find(1)!,
          imgUrl = "https://via.placeholder.com/300x300/000080/FFFFFF?text=Chaqueta",
          CreationDate = DateTime.Now
        },
        new Product
        {
          Name = "Auriculares Bluetooth",
          description = "Auriculares inalámbricos con cancelación de ruido",
          price = 189.99m,
          SKU = "PROD-012-AUR-BLK",
          Stock = 45,
          CategoryId = 2,
          Category = appContext.Categories.Find(2)!,
          imgUrl = "https://via.placeholder.com/300x300/1C1C1C/FFFFFF?text=Auriculares",
          CreationDate = DateTime.Now
        }
      );
    }
    appContext.SaveChanges();
    }
}