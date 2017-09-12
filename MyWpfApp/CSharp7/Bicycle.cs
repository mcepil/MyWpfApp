namespace MyWpfApp.CSharp7
{
    public class Bicycle : IVehicle
    {
        public int Wheels { get; private set; } = 2;
        public int Doors { get; } = 0;
        public decimal TimeFactor { get; } = 10;

        public Bicycle()
        {
        }

        public Bicycle(int wheels)
        {
            Wheels = wheels;
        }
    }
}
