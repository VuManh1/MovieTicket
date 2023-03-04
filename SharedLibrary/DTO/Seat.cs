namespace SharedLibrary.DTO
{
	public class Seat
	{
#pragma warning disable
		public int Id { get; set; }
		public char SeatRow { get; set; }
		public int SeatNumber { get; set; }
		public int Position { get; set; }
		public SeatType SeatType { get; set; }
		public double Price { get; set; }
		public Hall Hall { get; set; }

		public string SeatName
		{
			get => $"{SeatRow}{SeatNumber}";
		}
	}

	public enum SeatType
	{
		NORMAL, VIP
	}
}
