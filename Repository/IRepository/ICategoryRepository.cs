using System;

namespace back_net.Repository.IRepository
{    
    public interface ICategoryRepository
    {
        ICollection<Category> GetAllCategories();
        Category GetCategoryById(int id);
        bool CategoryExists(int id);
        bool CategoryExists(string nombre);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(int id);
        bool Save();
    }
}