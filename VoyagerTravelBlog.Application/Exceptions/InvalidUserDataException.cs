namespace VoyagerTravelBlog.Application.Exceptions
{
    public class InvalidUserDataException : Exception
    {
        public InvalidUserDataException(string message) 
            : base(message)
        {    
        }
    }
}
