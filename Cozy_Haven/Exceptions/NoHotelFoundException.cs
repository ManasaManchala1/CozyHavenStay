namespace Cozy_Haven.Exceptions
{
    public class NoHotelFoundException:Exception
    {
        string message;
        public NoHotelFoundException()
        {
            message = "No Hotel Found.";
        }
        public NoHotelFoundException(int id)
        {
            message = $"No Hotel found with id {id}";
        }
        public override string Message => message;

    }
}
