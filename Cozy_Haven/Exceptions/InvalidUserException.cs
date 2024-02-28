namespace Cozy_Haven.Exceptions
{
    public class InvalidUserException:Exception
    {
        string message;
        public InvalidUserException()
        {
            message = "Invalid Username or Password";
        }
        public override string  Message=>message;
    }
}
