using SuperMarketMini.Domain;
using SuperMarketMini.Repository;
using SuperMarketMini.Servies.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketMini.Servies
{
    public  class SupCatServices
    {
        private IValidationDictionary _validationDictionary;
        private ISupplierRepository _supplierRepository;
        private ICategoryRepository _categoryRepository;
        public SupCatServices(IValidationDictionary validationDictionary)
        {
            _validationDictionary = validationDictionary;
            _categoryRepository = new CategoryRepository();
            _supplierRepository = new SupplierRepository();
        }
        public bool createCat(Category target)
        {
            try
            {
                _categoryRepository.createCategory(target);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public bool updateCat(Category target)
        {
            try
            {
                _categoryRepository.updateCategory(target);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public bool deleteCat(Category target)
        {
            try
            {
                _categoryRepository.deleteCategory(target);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public Category getCat(string ID)
        {
            return _categoryRepository.getCategory(ID);
        }
        public IEnumerable<Category> listCat()
        {
            return _categoryRepository.listCategory();
        }
        public IEnumerable<Category> searchCatByKey(string key)
        {
            List<Category> _list = _categoryRepository.listCategory().ToList();
            if (String.IsNullOrEmpty(key))
                return _list;
            if (key.ToUpper().Equals("Key: Name, GroupName".ToUpper()))
            {
                return _list;
            }
            IEnumerable<Category> result = _list.Where(c => c.CategoryID.ToUpper().Contains(key.ToUpper()) || c.Name.ToUpper().Contains(key.ToUpper()) || c.GroupName.ToUpper().Contains(key.ToUpper()));
            return result;
        }
        public bool createSup(Supplier target)
        {
            try
            {
                _supplierRepository.createSupplier(target);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public bool updateSup(Supplier target)
        {
            try
            {
                _supplierRepository.updateSupplier(target);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public bool deleteSup(Supplier target)
        {
            try
            {
                _supplierRepository.deleteSupplier(target);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public Supplier getSup(string ID)
        {
            return _supplierRepository.getSupplier(ID);
        }
        public IEnumerable<Supplier> listSup()
        {
            return _supplierRepository.listSupplier();
        }
        public IEnumerable<Supplier> searchSupByKey(string key)
        {
            List<Supplier> _list = _supplierRepository.listSupplier().ToList();
            if (String.IsNullOrEmpty(key))
                return _list;
            if (key.ToUpper().Equals("Key: Name, Email, Phone".ToUpper()))
            {
                return _list;
            }
            IEnumerable<Supplier> result = _list.Where(c => c.SupplierID.ToUpper().Contains(key.ToUpper())
            || c.Name.ToUpper().Contains(key.ToUpper())
            || c.Email.ToUpper().Contains(key.ToUpper())
            || c.Phone.Contains(key)
            );
            return result;
        }
    }
}
