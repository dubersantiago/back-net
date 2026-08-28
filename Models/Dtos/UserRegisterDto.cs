namespace back_net.Models.Dtos;

public class UserRegisterDto{
    public String? Id {get;set;}
    public String? Name {get;set;}
    public required String Username {get;set;}
    public required String Password {get;set;}
    public String? Rol {get;set;}
}