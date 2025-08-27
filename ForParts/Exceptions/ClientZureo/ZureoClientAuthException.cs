namespace ForParts.Exceptions.ClientZureo
{
    public class ZureoClientAuthException : ZureoClientException
    {
        public ZureoClientAuthException() { }
        public ZureoClientAuthException(string message, string errorBody) : base(message) { }
    }
}
