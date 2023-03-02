namespace SharedLibrary.DTO
{
    public class Movie
    {
#pragma warning disable
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizeName { get; set; }
        public string? Description { get; set; }
        public int Length { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public MovieStatus MovieStatus { get; set; }
        public string? Country { get; set; }

        public string? CastIdString { get; set; }
        public string? DirectorIdString { get; set; }
        public string? GenreIdString { get; set; }
    }

    public enum MovieStatus
    {
        Playing, Coming, Stop
    }
}

