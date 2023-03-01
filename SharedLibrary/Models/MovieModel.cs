using SharedLibrary.DTO;

namespace SharedLibrary.Models
{
    public class MovieModel
    {
#pragma warning disable
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Length { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string? Poster { get; set; }
        public MovieStatus MovieStatus { get; set; }

        public string Country { get; set; }
        public string? Casts { get; set; }
        public string? Directors { get; set; }
    }
}
