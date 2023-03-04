using SharedLibrary.DTO;

namespace SharedLibrary.Models
{
    public class BookingModel
    {
#nullable disable
        public Booking Booking { get; set; }
        public Show Show { get; set; }
        public List<Seat> Seats { get; set; }
    }
}
