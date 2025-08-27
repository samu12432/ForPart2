namespace ForParts.Exceptions.Auth
{
    public class UserNameException : UserException
    {
        public UserNameException() { }
        public UserNameException(string message) : base(message) { }
    }
}
