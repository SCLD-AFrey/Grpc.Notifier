using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Notification.Common
{
    public class UserEngine
    {
        public class User
        {
            public string UserName { get; set; }
            public List<emRoles> Roles { get; set; }
        }

        [Flags]
        public enum emRoles : byte
        {
            ADMIN = 1,
            POWERUSER = 2,
            USER = 4
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();
            
            users.Add(new User() {UserName = "afrey", Roles = {emRoles.ADMIN}});
            users.Add(new User() {UserName = "user1", Roles = {emRoles.POWERUSER}});
            users.Add(new User() {UserName = "user2", Roles = {emRoles.USER}});

            return users;
        }

        public User GetUserByUsername(string p_username)
        {
            var users = GetUsers();
            var user = (User)users.Where(o => o.UserName.ToLower() == p_username.ToLower());
            return user;
        }
        
    }
}