using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace back_net.Models;

public class ApplicationUser: IdentityUser
{
    public String? name { get; set; }
}