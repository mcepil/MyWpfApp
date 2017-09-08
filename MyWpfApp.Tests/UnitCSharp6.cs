using MyWpfApp.CSharp6;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MyWpfApp.Tests
{
    public class UnitCSharp6
    {
        [Fact]
        public void AssertUserHasId()
        {
            var user = new User();
            Assert.NotNull(user.Id);
            Assert.NotEqual(user.Id, Guid.Empty);
        }

        [Fact]
        public void AssertUserCreatesDictionary()
        {
            var user = new User();
            var users = user.GetDictionary();
            Assert.NotNull(users);
            Assert.NotEmpty(users);
            Assert.All(new[] { "user1", "user2" }, (key) => users.ContainsKey(key));
        }

        [Fact]
        public void AssertEventInitializer()
        {
            User eventUser = null;
            var user = new User()
            {
                Name = "TestUser",
                //was planned to work this way, but was scrapped
                //Speaking += (o, e) => eventUser = o as User
            };
            user.Speaking += (o, e) => eventUser = o as User;

            user.Speak();

            Assert.NotNull(eventUser);
            Assert.Equal(user.Id, eventUser.Id);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AssertStringInterpolation(bool appendString)
        {
            var a = "interpolated";
            var b = "string";

            var expected = "interpolated string{s}";
            if (appendString)
                expected += " are awesome";

            Assert.Equal(expected, $"{a,-13}{b}{{s}}{(appendString ? " are awesome" : "")}");
        }

        [Fact]
        public void AssertPlusOperator()
        {
            var user1 = new User();
            var user2 = new User();
            var summedUser = user1 + user2;

            Assert.Equal(user1.Name + user2.Name, summedUser.Name);
        }

        [Fact]
        public void AssertImplicitOperator()
        {
            var user = new User();

            Assert.Equal("Id:" + user.Id + ", Name:" + user.Name, (string)user);
        }

        [Fact]
        public void AssertIndexer()
        {
            var index = 111;
            var user = new User();

            Assert.Equal(user.Name + "[" + index + "]", user[index].Name);
        }

        [Fact]
        public void AssertExceptionFilter()
        {
            var user = new User();

            Assert.Equal("test", user.FilterException(true));
        }

        [Fact]
        public void AssertExceptionNoFilter()
        {
            var user = new User();

            var ex = Record.Exception(() => user.FilterException(false));

            Assert.IsType(typeof(ApplicationException), ex);
            Assert.Equal("test", ex.Message);
        }

        [Fact]
        public void AssertAwaitInTryFakeUri()
        {
            var user = new User();

            var response = Task.Run(() => user.AwaitInTryCatch("http://fake.uri.that.never.existed.ever")).Result;

            Assert.StartsWith("AwaitInTryCatch exception caught:", response);
        }

        [Fact]
        public void AssertAwaitInTryRealUri()
        {
            var user = new User();

            var response = Task.Run(() => user.AwaitInTryCatch("http://google.com")).Result;

            Assert.NotNull(response);
            Assert.DoesNotContain("AwaitInTryCatch exception caught:", response);
        }

        [Fact]
        public void AssersUsersCanAddUser()
        {
            //empty collection
            var users1 = new Users();
            //this initializer is using the extension Add() method of Users class,
            //because the defult adding method in Users is called AddUser (not Add)
            var users2 = new Users()
            {
                new User() { Name = "user1" },
                new User() { Name = "user2" },
                new User() { Name = "user3" }
            };

            Assert.Equal(0, users1.Count());
            Assert.Equal(3, users2.Count());
        }
    }
}
