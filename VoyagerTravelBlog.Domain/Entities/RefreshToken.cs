namespace VoyagerTravelBlog.Domain.Entities
{
    public class RefreshToken
    {
        public required int Id { get; set; }

        public int UserId {  get; set; }

        public required string Token {  get; set; }

        public DateTimeOffset ExpiryDate { get; set; }

        //Relationships
        public required User User { get; set; }
    }
}
