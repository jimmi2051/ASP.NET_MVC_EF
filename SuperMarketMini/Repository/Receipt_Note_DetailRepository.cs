using System.Collections.Generic;
using System.Linq;
using SuperMarketMini.Domain;
namespace SuperMarketMini.Repository
{
    public class Receipt_Note_DetailRepository : IReceipt_Note_DetailRepository
    {
        private SuperMarketMini_Context db = new SuperMarketMini_Context();
        public Receipt_Note_Detail createReceipt_Note_Detail(Receipt_Note_Detail target)
        {
            db.Receipt_Note_Detail.Add(target);
            db.SaveChanges();
            return target;
        }

        public void deleteReceipt_Note_Detail(Receipt_Note_Detail target)
        {
            db.Receipt_Note_Detail.Remove(target);
            db.SaveChanges();
        }

        public Receipt_Note_Detail getReceipt_Note_Detail(string RID, string PID)
        {
            return db.Receipt_Note_Detail.Where(c => c.Receipt_NoteID.Equals(RID)&&c.ProductID.Equals(PID)).FirstOrDefault();
        }

        public IEnumerable<Receipt_Note_Detail> listReceipt_Note_Detail()
        {
            return db.Receipt_Note_Detail.ToList();
        }

        public IEnumerable<Receipt_Note_Detail> listReceipt_Note_DetailByID(string RID)
        {
            return db.Receipt_Note_Detail.Where(c => c.Receipt_NoteID.Equals(RID)).ToList();
        }

        public Receipt_Note_Detail updateReceipt_Note_Detail(Receipt_Note_Detail target)
        {
            var current = getReceipt_Note_Detail(target.Receipt_NoteID,target.ProductID);
            db.Entry(current).CurrentValues.SetValues(target);
            db.SaveChanges();
            return target;
        }
    }
}
