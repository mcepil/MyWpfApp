namespace MyWpfApp.CSharp6
{
    public static class UsersExtensions
    {
        //This extension will be used in Users initializers, since the class
        //itself does not define a Add() method - this wasn't possible before C# 6
        //<see test AssersUsersCanAddUser>
        public static User Add(this Users users, User newUser)
        {
            return users.AddUser(newUser);
        }
    }
}
