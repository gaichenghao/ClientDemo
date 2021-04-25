using Model;
using System;
using System.Collections.Generic;

namespace Interface
{
    public interface IUserService
    {
        public User FindUser(int id);

        public IEnumerable<User> UserAll();
    }
}
