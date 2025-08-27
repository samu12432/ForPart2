namespace ForParts.Exceptions.ClientZureo
{
    public class ZureoClientApiException : ZureoClientException
    {
        public ZureoClientApiException() { }
        public ZureoClientApiException(string message, string errorBody) : base(message) { }
    }
}
