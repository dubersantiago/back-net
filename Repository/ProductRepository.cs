using back_net.Models;
using back_net.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace back_net.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db)
    {
        this._db=db;
    }
    public ICollection<Product> GetProducts()
    {
        return _db.products.Include(p=>p.Category).OrderBy(p=>p.Name).ToList();
    }
    public ICollection<Product> GetProductsForCategory(int categoryId)
    {
        if(categoryId<=0) return new List<Product>();
        return _db.products.Include(p=>p.Category).Where(p=>p.CategoryId==categoryId).OrderBy(p=>p.Name).ToList();
    }
    public Product? GetProduct(int id)
    {
        if(id<=0) return null;
        return _db.products.Include(p=>p.Category).FirstOrDefault(p=>p.ProductId==id);
    }
    public bool BuyProduct(string nombre, int cantidad)
    {
        if(String.IsNullOrWhiteSpace(nombre) || cantidad<=0) return false;
        
        var product = _db.products.FirstOrDefault(p=>p.Name.ToLower().Trim() == nombre.ToLower().Trim());
        if(product == null || product.Stock<cantidad) return false;

        product.Stock-=cantidad;

        _db.products.Update(product);

        return Save();
    }

    public bool CreateProduct(Product product)
    {
        if(product==null)return false;
        product.CreationDate=DateTime.Now;
        product.UpdateDate=null;
        _db.products.Add(product);
        return Save();
    }

    public bool DeleteProduct(Product product)
    {
        if(product==null)return false;
        _db.products.Remove(product);
        return Save();
    }
    public bool ProductExist(int id)
    {
        if(id<=0) return false;
        return _db.products.Any(p=>p.ProductId == id);
    }
    public bool ProductExist(String nombre)
    {
        if(String.IsNullOrWhiteSpace(nombre)) return false;
        return _db.products.Any(p=>p.Name.ToLower().Trim() == nombre.ToLower().Trim());
    }
    public ICollection<Product> SearchProduct(string nombre)
    {
        IQueryable<Product> query = _db.products;
        if (!String.IsNullOrEmpty(nombre))
        {
            query = query.Where(p=>p.Name.ToLower().Trim() == nombre.ToLower().Trim());
        }
        return query.OrderBy(p=>p.Name).ToList();
    }
    public bool UpdateProduct(Product product)
    {
        if(product==null)return false;
        product.UpdateDate=DateTime.Now;
        _db.products.Update(product);
        return Save();
    }
    public bool Save()
    {
        return _db.SaveChanges() >= 0 ; ;
    }
}