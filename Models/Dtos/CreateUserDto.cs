using System.ComponentModel.DataAnnotations;

namespace back_net.Models.Dtos;
public class CreateUserDto
{
  [Required(ErrorMessage = "El campo username es requerido")]
  public string? Username { get; set; }
  [Required(ErrorMessage = "El campo name es requerido")]
  public string? Name { get; set; }
  [Required(ErrorMessage = "El campo password es requerido")]
  public string? Password { get; set; }
  [Required(ErrorMessage = "El campo role es requerido")]
  public string? Role { get; set; }
}