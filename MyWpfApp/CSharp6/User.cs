using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Diagnostics.Debug;

namespace MyWpfApp.CSharp6
{
    /// <summary>
    /// C# 6.0 features playground
    /// </summary>
    public class User
    {
        //init param without a setter - "Auto Property Initializer"
        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; set; } = String.Empty;

        public event EventHandler<EventArgs> Speaking;

        //operators defined as lambdas *and* using string interpolation
        public static User operator +(User a, User b) => new User() { Name = $"{a.Name}{b.Name}" };
        //this one is using the *nameof* expression additionaly
        public static implicit operator string(User u) => $"{nameof(u.Id)}:{u.Id}, {nameof(u.Name)}:{u.Name}";

        //property (indexer in this case) defined as lambda
        public User this[int index] => new User { Name = $"{this.Name}[{index}]" };

        public void DoSomething()
        {
            //static using - this is calling System.Diagnostics.Debug.WriteLine
            WriteLine("Something");
        }

        //method defined using lambda
        public void DoSomethingLambda(string param) => WriteLine($"Something lambda: {param}");

        public Dictionary<string, User> GetDictionary()
        {
            //Index initializer - new Dictionary initializer (which uses indexer syntax for adding dictionary 
            //items in the compiled code, instead of the Add() method, which was used with the older {} syntax)
            return new Dictionary<string, User>()
            {
                ["user1"] = new User(),
                ["user2"] = new User()
            };

            //this is what it would've looked like using the older syntax
            //return new Dictionary<string, User>()
            //{
            //    { "user1", new User() },
            //    { "user2", new User() }
            //};
        }

        public void Speak()
        {
            //null-conditional operator
            Speaking?.Invoke(this, new EventArgs());
        }

        public string FilterException(bool doFilter)
        {
            try
            {
                throw new ApplicationException("test");
            }
            catch (ApplicationException ex) when (doFilter) //this is the exception filter (can be a method call)
            {
                return ex.Message;
            }
        }

        public async Task<string> AwaitInTryCatch(string uri)
        {
            HttpClient client = new HttpClient();
            try
            {
                return await client.GetStringAsync(uri);
            }
            catch (Exception ex)
            {
                var message = $"AwaitInTryCatch exception caught: {ex.Message}";
                WriteLine(message);
                //previously it was not possible to await inside of try block (in C# 6 you can await in finally too)
                var fallbackResponse = await client.GetStringAsync("https://github.com/");
                return $"{message}, fallback url reponse: {fallbackResponse}";
            }
        }
    }
}
