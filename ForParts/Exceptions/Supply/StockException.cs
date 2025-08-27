namespace ForParts.Exceptions.Supply
{
    public class StockException : Exception
    {
        public StockException() { }
        public StockException(string message) : base(message) { }
    }
}
