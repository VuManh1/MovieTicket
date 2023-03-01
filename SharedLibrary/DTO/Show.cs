namespace SharedLibrary.DTO
{
	public class Show
	{
#pragma warning disable
		public int Id { get; set; }
		public Movie Movie { get; set; }
		public Hall Hall { get; set; }
		public DateTime StartTime { get; set; }
	}
}
