namespace SharedLibrary.DTO
{
	public class Seat
	{
#pragma warning disable
		public string Id { get; set; }
		public int SeatNumber { get; set; }
		public SeatType SeatType { get; set; }
		public double Price { get; set; }
		public Hall Hall { get; set; }
	}

	public enum SeatType
	{
		Normal, VIP
	}
}
