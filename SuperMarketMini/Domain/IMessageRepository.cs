using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketMini.Domain
{
    public interface IMessageRepository
    {
        Message createMess(Message target);
        Message updateMess(Message target);
        void deleteMess(Message target);
        Message getMess(int id);
        IEnumerable<Message> listMess();
    }
}
