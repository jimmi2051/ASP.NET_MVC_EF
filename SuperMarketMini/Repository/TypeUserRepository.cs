using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketMini.Domain;
namespace SuperMarketMini.Repository
{
    public class TypeUserRepository : ITypeUserRepository
    {
        private SuperMarketMini_Context db = new SuperMarketMini_Context();
        public TypeUser createType(TypeUser index)
        {
            db.TyperUsers.Add(index);
            db.SaveChanges();
            return index;
        }

        public void deleteType(TypeUser index)
        {
            db.TyperUsers.Remove(index);
            db.SaveChanges();
        }

        public TypeUser getType(string TID)
        {
            return db.TyperUsers.Where(c => c.TypeID.Equals(TID)).FirstOrDefault();
        }

        public IEnumerable<TypeUser> listType()
        {
            return db.TyperUsers.ToList();
        }

        public TypeUser updateType(TypeUser index)
        {
            TypeUser currentUser = getType(index.TypeID);
            db.Entry(currentUser).CurrentValues.SetValues(index);
            db.SaveChanges();
            return index;
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
