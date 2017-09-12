using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace MyWpfApp.CSharp7
{
    /// <summary>
    /// C# 7.0 features playground
    /// </summary>
    public class Car : IVehicle
    {
        public int Wheels { get; private set; } = 2;
        public int Doors { get; private set; } = 4;
        public int Fuel { get; set; } = 0;
        public int FuelTankCapacity { get; } = 100;
        public decimal Mileage { get; set; } = 30;
        public decimal TimeFactor { get; } = 1;

        public Car()
        {
        }

        public Car(int wheels, int doors, decimal mileage)
        {
            Wheels = wheels;
            Doors = doors;
            Mileage = mileage;
        }

        //improved Tuple - tuple literal (using meaningful names, instead of item1, etc.; this is also a value type)
        // (!) this needs at least .NET Framework 4.7
        public (int fuel, decimal range) GetRangeInfo() => (Fuel, Fuel * Mileage);

        public decimal GetRange()
        {
            //deconstruction *and* discard declaration ("_")
            (_, var range) = GetRangeInfo();
            return range;
        }

        public bool CanGoOnRoute(IRoute route)
        {
            //deconstruction works on objects too (not only on Tuples)
            //the class has to implement a Deconstruct method
            (var distance, _) = route;
            return GetRange() >= distance;
        }

        public bool CanGoOnRoutes(IEnumerable<IRoute> routes)
        {
            var range = GetRange();
            var distance = 0m;
            //local functions - limits scope and visibility (also does not allocate additional memory on heap or stack)
            bool CheckRoute(IRoute route)
            {
                if (range >= distance + route.Distance)
                {
                    distance += route.Distance;
                    return true;
                }
                return false;
            }

            var canGo = true;
            foreach (IRoute route in routes)
            {
                canGo &= CheckRoute(route);
            }
            return canGo;
        }

        public ref IRoute FindFirstOutOfLimitRoute(IRoute[] routes)
        {
            var range = GetRange();
            for (var i = 0; i < routes.Count(); i++)
            {
                if (routes[i].Distance > range)
                    return ref routes[i];
            }
            throw new ArgumentException("No route found");
        }

        //ValueTask - as a non reference type returned from async method 
        //(for now this needs System.Threading.Tasks.Extensions NuGet package)
        private string cacheResult;
        public ValueTask<string> CachedAwaitInTryCatch(string uri) 
            => string.IsNullOrWhiteSpace(cacheResult) ? new ValueTask<string>(AwaitInTryCatch(uri)) : new ValueTask<string>(cacheResult);
        private async Task<string> AwaitInTryCatch(string uri)
        {
            HttpClient client = new HttpClient();
            try
            {
                cacheResult = await client.GetStringAsync(uri);
                return cacheResult;
            }
            catch (Exception ex)
            {
                return $"AwaitInTryCatch exception caught: {ex.Message}";
            }
        }
    }
}
