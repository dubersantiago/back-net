namespace back_net.Models.Dtos;

public class ProductDto
{
    public int ProductId { get;set; }
    public String Name { get;set; } = String.Empty;
    public String description { get;set; } = String.Empty;
    public decimal price { get; set; }
    public string imgUrl { get; set; } = String.Empty;
    public string SKU { get; set; } = String.Empty;
    public int Stock { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; } = null;
    public int CategoryId { get; set; }
    public String CategoryName { get; set; }= String.Empty;

}