using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.BookingView
{
    public class AddBookingView : IViewRender
    {
		private readonly BookingBUS _bookingBUS;
        private readonly IViewFactory _viewFactory;

        public AddBookingView(BookingBUS bookingBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _bookingBUS = bookingBUS;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Booking \n[/]");

            Booking booking = new();
            
            booking.Show.Id = AnsiConsole.Ask<int>(" -> Enter Show Id: ");

            booking.SeatCount = AnsiConsole.Ask<int>(" -> Enter SeatCount: ");

            booking.User.Id = AnsiConsole.Ask<int>(" -> Enter User Id: ");
            
            booking.CreateTime = AnsiConsole.Ask<DateTime>(" -> Enter CreateTime: ");

            booking.Total = AnsiConsole.Ask<double>(" -> Enter Total: ");


            Result result = _bookingBUS.Create(booking);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add Booking successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.Render(ViewConstant.ManageBooking);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.Render(ViewConstant.ManageBooking);
                    return;
                }

                _viewFactory.Render(ViewConstant.AddBooking);
            }
        }
    }
}