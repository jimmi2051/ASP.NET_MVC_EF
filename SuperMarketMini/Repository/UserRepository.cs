using System.Collections.Generic;
using System.Linq;
using SuperMarketMini.Domain;
namespace SuperMarketMini.Repository
{
    public class UserRepository : IUserRepository
    {
        SuperMarketMini_Context db = new SuperMarketMini_Context();
        public User createUser(User index)
        {
            db.Users.Add(index);
            db.SaveChanges();
            return index;
        }

        public void deleteUser(User index)
        {
            db.Users.Remove(index);
            db.SaveChanges();
        }

        public User getUser(string UID)
        {
            return db.Users.Where(c => c.Username.Equals(UID)).FirstOrDefault();
        }

        public IEnumerable<User> listUser()
        {
            return db.Users.ToList();
        }

        public User updateUser(User index)
        {
            User currentUser = getUser(index.Username);
            db.Entry(currentUser).CurrentValues.SetValues(index);
            db.SaveChanges();
            return index;
        }
        public User loginUser(string UID,string PW)
        {
            return db.Users.Where(c => c.Username.Equals(UID) && c.Password.Equals(PW)).FirstOrDefault();
        }
    }
}
