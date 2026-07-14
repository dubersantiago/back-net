using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
    public int id { get; set; }
    [Required]
    public String name {get;set;} = String.Empty;
    [Required]
    public DateTime creationDate {get;set;} 

}