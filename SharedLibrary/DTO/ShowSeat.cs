namespace SharedLibrary.DTO
{
	public class ShowSeat
	{
#pragma warning disable
		public Seat Seat { get; set; }
		public Show Show { get; set; }
		public Booking? Booking { get; set; }
		public SeatStatus SeatStatus { get; set; }
	}

	public enum SeatStatus
	{
		Empty, Picked
	}
}
