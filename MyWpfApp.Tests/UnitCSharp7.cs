using MyWpfApp.CSharp7;
using System;
using System.Globalization;
using System.Linq;
using Xunit;

namespace MyWpfApp.Tests
{
    public class UnitCSharp7
    {
        #region Car tests
        [Theory]
        [InlineData(4,5,33)]
        [InlineData(3,2,50)]
        public void AssertCarConstructor(int wheels, int doors, decimal mileage)
        {
            Car car;

            car = new Car(wheels, doors, mileage);

            Assert.Equal(wheels, car.Wheels);
            Assert.Equal(doors, car.Doors);
            Assert.Equal(mileage, car.Mileage);
        }
        
        [Fact]
        public void AssertCarHasRangeInfo()
        {
            var car = new Car();
            car.Refuel();

            var rangeInfo = car.GetRangeInfo();

            Assert.NotNull(rangeInfo.fuel);
            Assert.True(rangeInfo.fuel > 0);
            Assert.NotNull(rangeInfo.range);
            Assert.True(rangeInfo.range > 0);
        }

        [Fact]
        public void AssertCarHasRange()
        {
            var car = new Car();
            car.Refuel();

            var range = car.GetRange();

            Assert.NotNull(range);
            Assert.True(range > 0);
        }

        [Theory]
        [InlineData(true, true, 500)]
        [InlineData(true, false, 3001)]
        [InlineData(false, false, 16500)]
        public void AssertCarGoes(bool doRefuel, bool carShouldGo, decimal distance)
        {
            var car = new Car();
            var route = new Route(distance);

            if (doRefuel)
                car.Refuel();
            car.Go(route);

            Assert.Equal(carShouldGo, route.ETA > 0);
            Assert.Equal(carShouldGo, car.Fuel < car.FuelTankCapacity && car.Fuel > 0);
        }

        [Theory]
        [InlineData(4555, true)]
        [InlineData(5000, true)]
        [InlineData(5001, false)]
        public void AssertCarCanGoOnRoute(decimal distance, bool carShouldBeAbleToGo)
        {
            var car = new Car(4, 4, 50);
            var route = new Route(distance);

            car.Refuel();
            var canGo = car.CanGoOnRoute(route);

            Assert.Equal(carShouldBeAbleToGo, canGo);
        }

        [Theory]
        [InlineData(true, new[] { "2000", "1000", "2000" })]
        [InlineData(true, new[] { "1000", "1000", "1000", "1000", "500.55", "498.45" })]
        [InlineData(false, new[] { "2000", "3001", "1000" })]
        public void AsserCarCanGoOnRoutes(bool carShouldBeAbleToGo, string[] routeDistances)
        {
            var car = new Car(4, 4, 50);
            var routes = (from routeDistance in routeDistances
                select new Route(Decimal.Parse(routeDistance, CultureInfo.InvariantCulture))).ToArray();

            car.Refuel();
            var canGo = car.CanGoOnRoutes(routes);

            Assert.Equal(carShouldBeAbleToGo, canGo);
        }

        [Fact]
        public void AssertRefChangesRoute()
        {
            var car = new Car();
            var routes = new IRoute[]
            {
                new Route(2100m),
                new Route(200m),
                new Route(30m),
                new Route(1300m),
                new Route(5050m),
                new Route(15m)
            };

            car.Refuel();
            //this has to be decalred as ref too - otherwise it copies the value
            ref var route = ref car.FindFirstOutOfLimitRoute(routes);
            route = new Route(110m);

            Assert.Equal(route.Distance, routes[4].Distance);
            Assert.Equal(route, routes[4]);
        }

        [Fact]
        public void AssertNoOutOfLimitRouteFound()
        {
            var car = new Car();

            car.Refuel();
            IRoute route;
            var ex = Record.Exception(() => route = car.FindFirstOutOfLimitRoute(new IRoute[0]));

            Assert.NotNull(ex);
            Assert.IsType(typeof(ArgumentException), ex);
        }

        [Fact]
        public void AssertAsyncAwaitThrows()
        {
            var car = new Car();

            var response = car.CachedAwaitInTryCatch("http://fake.google.com").Result;

            Assert.StartsWith("AwaitInTryCatch exception caught:", response);
        }

        [Fact]
        public void AssertAsyncCache()
        {
            var car = new Car();

            var response = car.CachedAwaitInTryCatch("http://google.com").Result;
            //this cache does not make much sense logically
            var cachedResponse = car.CachedAwaitInTryCatch("https://www.google.com/search?q=abc").Result;

            Assert.Equal(response, cachedResponse);
        }
        #endregion

        #region Route tests
        [Fact]
        public void AssertInlineOutVariable()
        {
            var route = new Route(150m);

            //you can now declare out variables in the argument list of a method call
            decimal etaTheOldWay;
            route.Deconstruct(out decimal dist, out etaTheOldWay);

            Assert.Equal(150m, dist);
        }

        [Fact]
        public void AssertRouteTrySetDistanceSetsDistance()
        {
            var route = new Route(150m);

            var set = route.TrySetDistance("1155");

            Assert.True(set);
            Assert.Equal(1155m, route.Distance);
        }

        [Fact]
        public void AssertRouteTrySetDistanceDoesntSetDistance()
        {
            var route = new Route(150m);

            var set = route.TrySetDistance("11-55");

            Assert.False(set);
            Assert.Equal(150m, route.Distance);
        }

        [Fact]
        public void AssertRouteThrowsFromExpression()
        {
            IRoute route;

            var ex = Record.Exception(() => route = new Route(-1));

            Assert.NotNull(ex);
            Assert.IsType(typeof(ArgumentException), ex);
        }
        #endregion

        #region Bicycle tests
        [Fact]
        public void AssertBicycleCreates()
        {
            var bicycle = new Bicycle();
            Assert.Equal(2, bicycle.Wheels);

            bicycle = new Bicycle(3);
            Assert.Equal(3, bicycle.Wheels);
        }

        [Theory]
        [InlineData(true, 500)]
        [InlineData(false, 16500)]
        public void AssertBicycleGoes(bool doRefuel, decimal distance)
        {
            var bicycle = new Bicycle();
            var route = new Route(distance);

            if (doRefuel)
                bicycle.Refuel();   //does nothing
            bicycle.Go(route);

            Assert.True(route.ETA > 0);
        }
        #endregion

        #region Skateboard tests
        [Fact]
        public void AssertSkateboardCreates()
        {
            var skateboard = new Skateboard();

            Assert.Equal(4, skateboard.Wheels);
        }

        [Fact]
        public void AssertSkateboardDoesNotGo()
        {
            var skateboard = new Skateboard();
            var route = new Route(5);

            skateboard.Go(route);    //does nothing
            
            Assert.Equal(0, route.ETA);
        }
        #endregion

        [Fact]
        public void AssertNonDefinedVehicleDoesNotGo()
        {
            IVehicle vehicle = null;
            var route = new Route(5);

            vehicle.Go(route);

            Assert.Equal(0, route.ETA);
        }

        [Fact]
        public void AssertNumericLiterals()
        {
            //numeric literal syntax improvements
            const int Sixteen = 0b0001_0000;
            Assert.Equal(16, Sixteen);

            const long BillionsAndBillions = 100_000_000_000;
            Assert.Equal(100000000000, BillionsAndBillions);

            const double AvogadroConstant = 6.022_140_857_747_474e23;
            Assert.Equal((6.022140857747474 * Math.Pow(10, 23)).ToString(), AvogadroConstant.ToString());

            const decimal GoldenRatio = 1.618_033_988_749_894_848_204_586_834_365_638_117_720_309_179M;
            Assert.Equal(1.6180339887498948482045868344m, GoldenRatio);
        }
    }
}
