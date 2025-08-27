namespace ForParts.Exceptions.Invoice
{
    public class InvoiceException : Exception
    {
        public InvoiceException() { }
        public InvoiceException(string message) : base(message) { }
    }
}
