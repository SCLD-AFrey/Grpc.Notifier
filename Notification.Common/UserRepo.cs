using System;
using System.Collections.ObjectModel;

namespace Notification.Common
{
    public static class UserRepo
    {
        public static Collection<ClientUser> Users()
        {
            Collection<ClientUser> users = new Collection<ClientUser>();
            users.Add(new ClientUser(){UserName = "afrey", Roles = new enRoles[] {enRoles.ADMIN}});
            users.Add(new ClientUser(){UserName = "user1", Roles = new enRoles[] {enRoles.POWERUSER}});
            users.Add(new ClientUser(){UserName = "user2", Roles = new enRoles[] {enRoles.USER}});
            return users;
        }
        [Flags]
        public enum enRoles : byte
        {
            ADMIN   = 1 << 0,
            POWERUSER   = 1 << 1, 
            USER = 1 << 2,
        }
        public class ClientUser
        {
            public string UserName { get; set; }
            public UserRepo.enRoles[] Roles { get; set; }
        }
    }
}