namespace SharedLibrary.DTO
{
	public class Booking
	{
#pragma warning disable
		public string Id { get; set; }
		public User User { get; set; }
		public Show Show { get; set; }
		public int SeatCount { get; set; }
		public DateTime CreateTime { get; set; }
		public double Total { get; set; }
	}
}
