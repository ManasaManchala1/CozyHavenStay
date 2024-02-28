namespace Cozy_Haven.Exceptions
{
    public class NoBookingFoundException:Exception
    {
        string message;
        public NoBookingFoundException()
        {
            message = "No Booking Found.";
        }
        public NoBookingFoundException(string username)
        {
            message = $"No Booking found for the {username}";
        }
        public NoBookingFoundException(int roomid)
        {
            message = $"No Booking found for room id {roomid}";
        }
        public override string Message => message;
    }
}
