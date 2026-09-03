using AutoMapper;
using back_net.Models;
using back_net.Models.Dtos;

namespace back_net.Mapping;

public class UserProfile:Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, CreateUserDto>().ReverseMap();
        CreateMap<User, UserLoginDto>().ReverseMap();
        CreateMap<User, UserLoginResponseDto>().ReverseMap();
        CreateMap<ApplicationUser, UserDataDto>().ReverseMap();
        CreateMap<ApplicationUser, UserDto>().ReverseMap();
    }
}