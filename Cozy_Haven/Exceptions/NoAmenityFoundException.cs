namespace Cozy_Haven.Exceptions
{
    public class NoAmenityFoundException:Exception
    {
        string message;
        public NoAmenityFoundException()
        {
            message = "No Amenity Found.";
        }
        public override string Message => message;
    }
}
