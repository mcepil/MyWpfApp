using System;

namespace MyWpfApp.CSharp7
{
    public class Route : IRoute
    {
        //constructors, finalizers and getters/setters can now be expressions too
        private decimal distance;
        public decimal Distance
        {
            get => distance;
            //throw expressions are now a thing
            private set => distance = value >= 0 ? value : throw new ArgumentException("Distance has to be 0 or more!");
        }
        public decimal ETA { get; set; }

        //constructors, finalizers and getters/setters can now be expressions too
        public Route(decimal distance) => Distance = distance;
        ~Route() => Console.WriteLine("Route finalized!");

        public void Deconstruct(out decimal distance, out decimal eta)
        {
            distance = Distance;
            eta = ETA;
        }

        public bool TrySetDistance(string distance)
        {
            //you can now declare out variables in the argument list of a method call
            if (decimal.TryParse(distance, out var dist))
            {
                Distance = dist;
                return true;
            }
            return false;
        }
    }
}
