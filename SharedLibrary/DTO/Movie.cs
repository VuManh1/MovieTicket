namespace SharedLibrary.DTO
{
    public class Movie
    {
#pragma warning disable
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizeName { get; set; }
        public string? Description { get; set; }
        public int Length { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public string? Poster { get; set; }

        public string Country { get; set; }
        public List<Cast>? Casts { get; set; }
        public List<Director>? Directors { get; set; }

	}

    public enum MovieStatus
    {
        Playing, Coming, Stop
    }
}

