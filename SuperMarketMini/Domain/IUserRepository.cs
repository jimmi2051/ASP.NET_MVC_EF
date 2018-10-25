using System;
using System.Collections.Generic;
namespace SuperMarketMini.Domain
{
    public interface IUserRepository
    {
        User createUser(User index);
        User updateUser(User index);
        void deleteUser(User index);
        IEnumerable<User> listUser();
        User getUser(String UID);
        User loginUser(String UID, String PW);
    }
}
