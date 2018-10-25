using System;
using System.Collections.Generic;

namespace SuperMarketMini.Domain
{
    public interface IReceipt_NoteRepository
    {
        Receipt_Note createReceipt_Note(Receipt_Note target);
        Receipt_Note updateReceipt_Note(Receipt_Note target);
        Receipt_Note getReceipt_Note(String SID);
        void deleteReceipt_Note(Receipt_Note target);
        IEnumerable<Receipt_Note> listReceipt_Note();
    }
}
