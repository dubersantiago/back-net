using System.ComponentModel.DataAnnotations;

namespace back_net.Models.Dtos;
public class PaginationResponse<T>
{
    public int Page {get; set;}
    public int Size {get;set;}
    public int TotalPages {get; set;}
    public ICollection<T> Items {get; set;} = new List<T>();
}