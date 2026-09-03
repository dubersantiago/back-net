namespace back_net.Models.Dtos;

public class UpdateProductDto
{
    public String Name { get;set; } = String.Empty;
    public String? description { get;set; } = String.Empty;
    public decimal price { get; set; }
    public string? imgUrl { get; set; }
    public string? imgUrlLocal { get; set; }
    public IFormFile? image {get;set;}
    public string SKU { get; set; } = String.Empty;
    public int Stock { get; set; }
    public int CategoryId { get; set; }

}