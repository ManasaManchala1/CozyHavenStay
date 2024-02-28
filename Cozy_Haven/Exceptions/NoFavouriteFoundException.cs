namespace Cozy_Haven.Exceptions
{
    public class NoFavouriteFoundException:Exception
    {
        string message;
        public NoFavouriteFoundException()
        {
            message = "No favourite found.";
        }
        public NoFavouriteFoundException(string username)
        {
            message = $"{username} have not added any favourites";
        }
        public override string Message => message;

    }
}
