namespace SharedLibrary.DTO
{
	public class Hall
	{
#pragma warning disable
		public int Id { get; set; }
		public Cinema Cinema { get; set; }
		public string Name { get; set; }
		public int SeatCount { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
    }
}
