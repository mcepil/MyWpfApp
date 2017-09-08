using System.Collections;
using System.Collections.Generic;

namespace MyWpfApp.CSharp6
{
    public class Users : IEnumerable<User>
    {
        List<User> users = new List<User>();

        public IEnumerator<User> GetEnumerator()
        {
            return users.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public User AddUser(User newUser)
        {
            users.Add(newUser);
            return newUser;
        }
    }
}
