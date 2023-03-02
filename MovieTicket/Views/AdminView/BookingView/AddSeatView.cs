using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.BookingView
{
    public class AddBookingView : IViewRender
    {
		private readonly BookingBUS _BookingBUS;
        private readonly IViewFactory _viewFactory;

        public AddBookingView(BookingBUS BookingBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _BookingBUS = BookingBUS;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            Booking Booking = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Booking \n[/]");
            
            Booking.Show.Id = AnsiConsole.Ask<int>(" -> Enter Show Id: ");

            Booking.SeatCount = AnsiConsole.Ask<int>(" -> Enter SeatCount: ");

            Booking.User.Id = AnsiConsole.Ask<int>(" -> Enter User Id: ");
            
            Booking.CreateTime = AnsiConsole.Ask<DateTime>(" -> Enter CreateTime: ");

            Booking.Total = AnsiConsole.Ask<double>(" -> Enter Total: ");


            Result result = _BookingBUS.AddBus(Booking);
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