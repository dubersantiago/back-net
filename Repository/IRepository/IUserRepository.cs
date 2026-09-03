using back_net.Models;
using back_net.Models.Dtos;

namespace back_net.Repository.IRepository;
public interface IUserRepository
{
    ICollection<ApplicationUser> GetUsers();
    ApplicationUser? GetUser(string id);
    bool IsUniqueUser(string name);
    Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
    Task<UserDataDto> Register(CreateUserDto createUserDto);
}