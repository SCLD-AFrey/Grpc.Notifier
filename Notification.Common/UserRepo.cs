using System.Collections.ObjectModel;

namespace Notification.Common
{
    public static class UserRepo
    {
        public static Collection<ClientUser> users()
        {
            Collection<ClientUser> users = new Collection<ClientUser>();
            users.Add(new ClientUser(){UserName = "afrey", Roles = new[]{"ADMIN"}});
            users.Add(new ClientUser(){UserName = "user1", Roles = new[]{"POWERUSER"}});
            users.Add(new ClientUser(){UserName = "user2", Roles = new[]{"USER"}});
            return users;
        }
    }
}