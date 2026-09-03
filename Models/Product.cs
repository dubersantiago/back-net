using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_net.Models;

public class Product
{
    [Key]
    public int ProductId { get;set; }
    [Required]
    public String Name { get;set; } = String.Empty;
    public String description { get;set; } = String.Empty;
    [Range(0,double.MaxValue)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal price { get; set; }
    public string? imgUrl { get; set; }
    public string? imgUrlLocal { get; set; }
    [Required]
    public string SKU { get; set; } = String.Empty;
    [Range(0,int.MaxValue)]
    public int Stock { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; } = null;

    // RELACION CON EL MODELO CATEGORY
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public required Category Category { get; set; }
}