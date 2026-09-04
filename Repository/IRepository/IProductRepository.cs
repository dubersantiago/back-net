using System;
using back_net.Models;

namespace back_net.Repository.IRepository;
public interface IProductRepository
{
    ICollection<Product> GetProducts();
    ICollection<Product> GetProductsInPages(int page, int size);
    int GetTotlaProducts();
    ICollection<Product> GetProductsForCategory(int categoryId);
    ICollection<Product> SearchProduct(String nombre);
    Product? GetProduct(int id);
    Boolean BuyProduct(String nombre, int cantidad);
    Boolean ProductExist(int id);
    Boolean ProductExist(String nombre);
    Boolean CreateProduct(Product product);
    Boolean UpdateProduct(Product product);
    Boolean DeleteProduct(Product product);
    Boolean Save();
}