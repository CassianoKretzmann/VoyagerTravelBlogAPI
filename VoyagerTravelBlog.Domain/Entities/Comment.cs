namespace VoyagerTravelBlog.Domain.Entities
{
	public class Comment
	{
		public int Id { get; set; }

        public required int UserId { get; set; }

        public required int PostId { get; set; }

        public required string Content { get; set; }

		public required DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? UpdatedAt { get; set; }

		// Relantions
		public required User User { get; set; }
		public required Post Post { get; set; }
	}
}

