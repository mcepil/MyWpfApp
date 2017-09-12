namespace MyWpfApp.CSharp7
{
    public interface IRoute
    {
        decimal Distance { get; }
        decimal ETA { get; set; }

        void Deconstruct(out decimal distance, out decimal eta);
    }
}
