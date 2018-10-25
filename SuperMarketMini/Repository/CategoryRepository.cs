using System.Collections.Generic;
using System.Linq;
using SuperMarketMini.Domain;
namespace SuperMarketMini.Repository
{
    public    class CategoryRepository : ICategoryRepository
    {
        private SuperMarketMini_Context db = new SuperMarketMini_Context();
        public Category createCategory(Category target)
        {
            db.Categories.Add(target);
            db.SaveChanges();
            return target;
        }

        public void deleteCategory(Category target)
        {
            db.Categories.Remove(target);
            db.SaveChanges();
        }

        public Category getCategory(string SID)
        {
            return db.Categories.Where(c => c.CategoryID.Equals(SID)).FirstOrDefault();
        }

        public IEnumerable<Category> listCategory()
        {
            return db.Categories.ToList();
        }

        public Category updateCategory(Category target)
        {
            var current = getCategory(target.CategoryID);
            db.Entry(current).CurrentValues.SetValues(target);
            db.SaveChanges();
            return target;
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }         
        }
    }
}
