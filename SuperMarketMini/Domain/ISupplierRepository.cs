using System;
using System.Collections.Generic;

namespace  SuperMarketMini.Domain
{
    public interface ISupplierRepository
    {
        Supplier createSupplier(Supplier target);
        Supplier updateSupplier(Supplier target);
        Supplier getSupplier(String SID);
        void deleteSupplier(Supplier target);
        IEnumerable<Supplier> listSupplier();
    }
}
