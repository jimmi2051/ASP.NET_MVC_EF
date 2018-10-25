
using System.Collections.Generic;
using System.Linq;
using SuperMarketMini.Domain;
namespace SuperMarketMini.Repository
{
    public class Receipt_NoteRepository : IReceipt_NoteRepository
    {
        private SuperMarketMini_Context db = new SuperMarketMini_Context();
        public Receipt_Note createReceipt_Note(Receipt_Note target)
        {
            db.Receipt_Note.Add(target);
            db.SaveChanges();
            return target;
        }

        public void deleteReceipt_Note(Receipt_Note target)
        {
            db.Receipt_Note.Remove(target);
            db.SaveChanges();
        }

        public Receipt_Note getReceipt_Note(string SID)
        {
            return db.Receipt_Note.Where(c => c.Receipt_NoteID.Equals(SID)).FirstOrDefault();
        }

        public IEnumerable<Receipt_Note> listReceipt_Note()
        {
            return db.Receipt_Note.ToList();
        }

        public Receipt_Note updateReceipt_Note(Receipt_Note target)
        {
            var current = getReceipt_Note(target.Receipt_NoteID);
            db.Entry(current).CurrentValues.SetValues(target);
            db.SaveChanges();
            return target;
        }
    }
}
