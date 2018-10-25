using SuperMarketMini.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketMini.Repository
{
    public class MessageRepository : IMessageRepository
    {
        SuperMarketMini_Context _db = new SuperMarketMini_Context();
        public Message createMess(Message target)
        {
            _db.Messages.Add(target);
            _db.SaveChanges();
            return target;
        }

        public void deleteMess(Message target)
        {
            _db.Messages.Remove(target);
            _db.SaveChanges();
        }

        public Message getMess(int id)
        {
            return _db.Messages.Where(c => c.Id.Equals(id)).FirstOrDefault();
        }

        public IEnumerable<Message> listMess()
        {
            return _db.Messages.ToList();
        }

        public Message updateMess(Message target)
        {
            var current = getMess(target.Id);
            _db.Entry(current).CurrentValues.SetValues(target);
            _db.SaveChanges();
            return target;
        }
    }
}
