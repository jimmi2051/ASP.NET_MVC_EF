using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SuperMarketMini.Domain;
using SuperMarketMini.Repository;
using SuperMarketMini.Servies.Validation;
namespace SuperMarketMini.Servies
{
    public class UserServices
    {
        private IValidationDictionary _validationDictionary;
        private IUserRepository _userRepository;
        private ITypeUserRepository _typeRepository;
        private List<User> _list;
        public UserServices(IValidationDictionary validationDictionary)
        {
            _validationDictionary = validationDictionary;
            _typeRepository = new TypeUserRepository();
            _userRepository = new UserRepository();
            _list = _userRepository.listUser().ToList();
        }
        //Kiểm tra dữ liệu
        public bool ValidateUser(User index)
        {
            List<User> listchecktele = _userRepository.listUser().Where(c => !String.IsNullOrEmpty(c.Phone)).ToList();
            _validationDictionary.Clear();
            if (_list.Where(c => c.Username.Equals(index.Username)).FirstOrDefault() != null)
                _validationDictionary.AddError("Username", "Username has already existed");
            if (_list.Where(c => c.Email.Equals(index.Email)).FirstOrDefault() != null)
                _validationDictionary.AddError("Email", "Email has already existed");
            if (index.Phone != null)
            {
                if (!Regex.IsMatch(index.Phone, @"^0+\d"))
                    _validationDictionary.AddError("Phone", "Phone is not valid");
                if(listchecktele.Where(c=>c.Phone.Equals(index.Phone)).FirstOrDefault()!=null)
                    _validationDictionary.AddError("Phone", "Phone has already existed");
            }
            if (index.Birthday != null)
                if (index.Birthday > DateTime.Now)
                    _validationDictionary.AddError("Birthday", "Birthday is not valid");
            return _validationDictionary.IsValid;
        }
        public bool ValidateUserUpdate(User index)
        {
            if (index.Birthday != null)
                if (index.Birthday > DateTime.Now)
                    _validationDictionary.AddError("Birthday", "Birthday is not valid");
            return _validationDictionary.IsValid;
        }
        public bool registerUser(User index)
        {
            if (!ValidateUser(index))
                return false;
            try
            {
                index.Created = DateTime.Now;
                index.Point = 0;
                index.Trust = 1000;
                index.Status = 2;
                index.TypeID = "CU";
                index.Password = Infrastructure.Encode.md5(index.Password);
                _userRepository.createUser(index);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool CheckPermissionAdmin(String UID)
        {
            User check = _userRepository.getUser(UID);
            if (check != null)
            {
                if (check.TypeID == "AD")
                    return true;
            }
            return false;
        }
        public IEnumerable<User> SearchUsers(String key)
        {
            _list = _userRepository.listUser().ToList();
            if (String.IsNullOrEmpty(key))
                return _list;
            if (key.ToUpper().Equals("Key: Username, DisplayName, Email".ToUpper()))
            {
                return _list;
            }
            try
            {
                IEnumerable<User> result;
                result = _list.FindAll(c => c.Username.ToUpper().Contains(key.ToUpper()) || c.Email.ToUpper().Contains(key.ToUpper()) || c.DisplayName.ToUpper().Contains(key.ToUpper()));
                return result;
            }
            catch
            {
                return _list;
            }
        }
        #region CRUD
        public bool createUser(User index)
        {
            if (!ValidateUser(index))
                return false;
            try
            {
                index.Created = DateTime.Now;
                index.Point = 0;
                index.Trust = 1000;
                index.Status = 1;
                index.Password = Infrastructure.Encode.md5(index.Password);
                _userRepository.createUser(index);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool updateUser(User index)
        {
            if (!ValidateUserUpdate(index))
                return false;
            try
            {
                User target = _userRepository.getUser(index.Username);
                if(index.Password!=target.Password)
                    index.Password = Infrastructure.Encode.md5(index.Password);
                _userRepository.updateUser(index);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool deleteUser(User index)
        {
            try
            {
                _userRepository.deleteUser(index);
            }
            catch
            {
                return false;
            }
            return true;

        }
        public IEnumerable<User> listUser()
        {
            return _userRepository.listUser().ToList();
        }
        public User getUser(String UID)
        {
            return _userRepository.getUser(UID);
        }
        public IEnumerable<TypeUser> listType()
        {
            return _typeRepository.listType().ToList();
        }
        public bool loginUser(String UID, String PW)
        {
            PW = Infrastructure.Encode.md5(PW);
            User target = _userRepository.loginUser(UID, PW);
            if (target != null && target.Status == 1)
                return true;
            return false;
        }
        public bool CheckExistsUser(User target)
        {
            List<User> listcheck = _userRepository.listUser().Where(c => !String.IsNullOrEmpty(c.Phone)).ToList();
            if (listcheck.Where(c => c.Phone.Equals(target.Phone) || c.Email.Equals(target.Email)).FirstOrDefault() != null)
                return true;
            return false;
        }
        public bool addUserFacebook(User target)
        {
            if (CheckExistsUser(target))
                return true;
            try
            {
                if (_userRepository.getUser(target.Username) == null)
                {
                    target.Status = 1;
                    target.Created = DateTime.Now;
                    target.TypeID = "CU";
                    target.Point = 1000;
                    target.Trust = 1000;
                    _userRepository.createUser(target);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion
        public bool UpdateImg(string Username,string Img)
        {
            User target = _userRepository.getUser(Username);
            target.Images = Img;
            _userRepository.updateUser(target);
            return true;
        }
        public TypeUser getTypeUser(String ID)
        {
            return _typeRepository.getType(ID);
        }
        public bool createTypeUser(TypeUser index)
        {
            try
            {
                _typeRepository.createType(index);
            }
            catch
            {
                return false;
            }
        
            return true;
        }
        public bool updateTypeUser(TypeUser index)
        {
            try
            {
                _typeRepository.updateType(index);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool deleteTypeUser(TypeUser index)
        {
            try
            {
                _typeRepository.deleteType(index);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public IEnumerable<TypeUser> SearchTypeUsers(String key)
        {
            List<TypeUser> _listtype = _typeRepository.listType().ToList();
            if (String.IsNullOrEmpty(key))
                return _listtype;
            if (key.ToUpper().Equals("Key: TypeID, Displayname".ToUpper()))
            {
                return _listtype;
            }
            try
            {
                IEnumerable<TypeUser> result;
                result = _listtype.FindAll(c => c.TypeID.ToUpper().Contains(key.ToUpper()) || c.DisplayName.ToUpper().Contains(key.ToUpper()));
                return result;
            }
            catch
            {
                return _listtype;
            }
        }
    }
}
