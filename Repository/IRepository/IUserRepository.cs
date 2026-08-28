using back_net.Models;
using back_net.Models.Dtos;

namespace back_net.Repository.IRepository;
public interface IUserRepository
{
    ICollection<User> GetUsers();
    User? GetUser(int id);
    bool IsUniqueUser(string name);
    Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
    Task<User> Register(CreateUserDto createUserDto);
}