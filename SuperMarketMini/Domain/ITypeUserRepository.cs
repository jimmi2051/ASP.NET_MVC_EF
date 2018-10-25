using System;
using System.Collections.Generic;

namespace SuperMarketMini.Domain
{
    public interface ITypeUserRepository

    {
        TypeUser createType(TypeUser index);
        TypeUser updateType(TypeUser index);
        void deleteType(TypeUser index);
        IEnumerable<TypeUser> listType();
        TypeUser getType(String TID);
    }
}
