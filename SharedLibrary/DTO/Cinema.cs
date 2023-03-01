namespace SharedLibrary.DTO
{
	public class Cinema
	{
#pragma warning disable
		public int Id { get; set; }
		public string Name { get; set; }
		public int HallCount { get; set; }
		public City? City { get; set; }
	}
}
