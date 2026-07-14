using back_net.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;

namespace back_net.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CategoryExists(int id)
        {
            return _db.Categories.Any(c => c.id ==id);
        }

        public bool CategoryExists(string nombre)
        {
            return _db.Categories.Any(c => c.name.ToLower().Trim() == nombre.ToLower().Trim());
        }

        public bool CreateCategory(Category category)
        {
            category.creationDate = DateTime.Now;
            _db.Categories.Add(category);
            return Save();
        }

        public bool DeleteCategory(int id)
        {
            _db.Categories.Remove(GetCategoryById(id));
            return Save();
        }

        public ICollection<Category> GetAllCategories()
        {
            return _db.Categories.OrderBy(c=>c.name).ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _db.Categories.FirstOrDefault(c=>c.id==id) ?? throw new InvalidOperationException($"La categoria con el id {id} no existe");
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false ;
        }

        public bool UpdateCategory(Category category)
        {
            category.creationDate = DateTime.Now;
            _db.Categories.Update(category);
            return Save();
        }
    }
}