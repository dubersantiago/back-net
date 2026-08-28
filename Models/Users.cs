using System.ComponentModel.DataAnnotations;

namespace back_net.Models;

public class User
{
    [Key]
    public int Id {get;set;}
    public String? Name {get;set;}
    public String Username {get;set;} = String.Empty;
    public String? Password {get;set;}
    public String? Rol {get;set;}
}