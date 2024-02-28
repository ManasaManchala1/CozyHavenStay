namespace Cozy_Haven.Exceptions
{
    public class NoRoomFoundException:Exception
    {
        string message;
        public NoRoomFoundException()
        {
            message = "No room found";
        }
        public override string Message => message;
    }
}
