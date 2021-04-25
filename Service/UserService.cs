using Interface;
using Model;
using System;
using System.Collections.Generic;

namespace Service
{
    public class UserService:IUserService
    {
        #region DataInit
        private List<User> _UserList = new List<User>() 
        {
            new User()
            {
                Id=1,Account="admin",Email="123@qq.com",Name="Gai",PassWord="123",LoginTime=DateTime.Now,Role="admin"
            },
            new User()
            {
                Id=2,Account="Apple",Email="123@qq.com",Name="apple",PassWord="123",LoginTime=DateTime.Now,Role="user"
            },
        };
        #endregion

        public User FindUser(int id)
        {
            return this._UserList.Find(x=>x.Id == id);
        }

        public IEnumerable<User> UserAll()
        {
            return this._UserList;
        }
    }
}
