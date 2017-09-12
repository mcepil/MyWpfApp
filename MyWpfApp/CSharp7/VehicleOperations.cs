using System;

namespace MyWpfApp.CSharp7
{
    public static class VehicleOperations
    {
        public static void Go(this IVehicle vehicle, IRoute route)
        {
            //switch with pattern matching
            switch (vehicle)
            {
                case Car car when car.Fuel > 0:
                    var fuelConsumption = (int)Math.Ceiling(route.Distance / car.Mileage);
                    if (car.Fuel >= fuelConsumption)
                    {
                        car.Fuel -= fuelConsumption;
                        route.ETA = route.Distance * car.TimeFactor;
                    }
                    break;
                case Bicycle bicycle:
                    route.ETA = route.Distance * bicycle.TimeFactor;
                    break;
            }
        }

        public static void Refuel(this IVehicle vehicle)
        {
            //pattern matching (no need to introduce an additional var)
            if (vehicle is Car car)
            {
                car.Fuel += car.FuelTankCapacity;
            }
        }
    }
}
