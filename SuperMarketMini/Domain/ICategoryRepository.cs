using System;
using System.Collections.Generic;

namespace SuperMarketMini.Domain
{
    public interface ICategoryRepository
    {
        Category createCategory(Category target);
        Category updateCategory(Category target);
        Category getCategory(String SID);
        void deleteCategory(Category target);
        IEnumerable<Category> listCategory();
    }
}
