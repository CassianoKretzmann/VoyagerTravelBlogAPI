namespace VoyagerTravelBlog.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key) 
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }

        public NotFoundException(string name1, string name2, object key)
            : base($"Entity \"{name1}\" was not found, for given \"{name2}\"({key}).")
        {
        }
    }
}
