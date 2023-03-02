using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.SeatView
{
    public class AddSeatView : IViewRender
    {
		private readonly SeatBUS _seatBUS;
        private readonly IViewFactory _viewFactory;

        public AddSeatView(SeatBUS seatBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _seatBUS = seatBUS;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            Seat seat = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Seat \n[/]");
            
            seat.Hall.Id = AnsiConsole.Ask<int>(" -> Enter Hall Id: ");

            seat.SeatNumber = AnsiConsole.Ask<int>(" -> Enter Seat Number: ");

            seat.SeatType = AnsiConsole.Ask<SeatType>(" -> Enter SeatType: ");
            
            seat.Price = AnsiConsole.Ask<double>(" -> Enter Price: ");


            Result result = _seatBUS.Create(seat);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add Seat successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.Render(ViewConstant.ManageSeat);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.Render(ViewConstant.ManageSeat);
                    return;
                }

                _viewFactory.Render(ViewConstant.AddSeat);
            }
        }
    }
}