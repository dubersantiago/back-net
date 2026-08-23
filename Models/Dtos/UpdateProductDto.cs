namespace back_net.Models.Dtos;

public class UpdateProductDto
{
    public String Name { get;set; } = String.Empty;
    public String? description { get;set; } = String.Empty;
    public decimal price { get; set; }
    public string imgUrl { get; set; } = String.Empty;
    public string SKU { get; set; } = String.Empty;
    public int Stock { get; set; }
    public int CategoryId { get; set; }

}