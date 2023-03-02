using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.ShowSeatView
{
    public class AddShowSeatView : IViewRender
    {
		private readonly ShowSeatBUS _ShowSeatBUS;
        private readonly IViewFactory _viewFactory;

        public AddShowSeatView(ShowSeatBUS ShowSeatBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _ShowSeatBUS = ShowSeatBUS;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            ShowSeat ShowSeat = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add ShowSeat \n[/]");
            
            ShowSeat.Seat.Id = AnsiConsole.Ask<int>(" -> Enter Seat Id: ");

            ShowSeat.Show.Id = AnsiConsole.Ask<int>(" -> Enter Show Id: ");

            ShowSeat.SeatStatus = AnsiConsole.Ask<SeatStatus>(" -> Enter SeatStatus: ");
            
            ShowSeat.Booking.Id = AnsiConsole.Ask<int>(" -> Enter Booking Id: ");


            Result result = _ShowSeatBUS.Create(ShowSeat);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add ShowSeat successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.Render(ViewConstant.ManageShowSeat);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.Render(ViewConstant.ManageShowSeat);
                    return;
                }

                _viewFactory.Render(ViewConstant.AddShowSeat);
            }
        }
    }
}