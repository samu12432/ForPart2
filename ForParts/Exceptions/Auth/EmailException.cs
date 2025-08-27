namespace ForParts.Exceptions.Auth
{
    public class EmailException : UserException
    {
        public EmailException() { }
        public EmailException(string message) : base(message){}    
    }
}