using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace back_net.Models.Dtos
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage ="El nombre es obligatorio")]
        [MaxLength(50, ErrorMessage ="El nombre no puede tener mas de 50 caracteres")]
        [MinLength(3,ErrorMessage ="El nombre no pude tener menos de 3 caracteres")]
        public String name {get;set;} = String.Empty;
    }
}