using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.SeatView
{
    public class AddSeatView : IViewRender
    {
		private readonly SeatBUS _SeatBUS;
        private readonly IViewFactory _viewFactory;

        public AddSeatView(SeatBUS SeatBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _SeatBUS = SeatBUS;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            Seat Seat = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Seat \n[/]");
            
            Seat.Hall.Id = AnsiConsole.Ask<int>(" -> Enter Hall Id: ");

            Seat.SeatNumber = AnsiConsole.Ask<int>(" -> Enter Seat Number: ");

            Seat.SeatType = AnsiConsole.Ask<SeatType>(" -> Enter SeatType: ");
            
            Seat.Price = AnsiConsole.Ask<double>(" -> Enter Price: ");


            Result result = _SeatBUS.Create(Seat);
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