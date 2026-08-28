
using back_net.Models;
using back_net.Models.Dtos;
using back_net.Repository.IRepository;

namespace back_net.Repository;
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db=db;
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

    public Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
    {
        throw new NotImplementedException();
    }

    public Task<User> Register(CreateUserDto createUserDto)
    {
        throw new NotImplementedException();
    }
}