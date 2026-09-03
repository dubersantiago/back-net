namespace back_net.Models.Dtos;
public class UserLoginResponseDto
{
    public UserDataDto? User {get;set;}
    public String? Token {get;set;}
    public String? Message {get;set;}
}