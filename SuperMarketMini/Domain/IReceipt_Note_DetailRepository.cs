using System;
using System.Collections.Generic;

namespace SuperMarketMini.Domain
{
    public interface IReceipt_Note_DetailRepository
    {
        Receipt_Note_Detail createReceipt_Note_Detail(Receipt_Note_Detail target);
        Receipt_Note_Detail updateReceipt_Note_Detail(Receipt_Note_Detail target);
        Receipt_Note_Detail getReceipt_Note_Detail(String RID,String PID);
        void deleteReceipt_Note_Detail(Receipt_Note_Detail target);
        IEnumerable<Receipt_Note_Detail> listReceipt_Note_Detail();
        IEnumerable<Receipt_Note_Detail> listReceipt_Note_DetailByID(String RID);
    }
}
