using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.ShowSeatView
{
    public class AddShowSeatView : IViewRender
    {
		private readonly ShowSeatBUS _showSeatBUS;
        private readonly IViewFactory _viewFactory;

        public AddShowSeatView(ShowSeatBUS showSeatBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _showSeatBUS = showSeatBUS;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            ShowSeat showSeat = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add ShowSeat \n[/]");
            
            showSeat.Seat.Id = AnsiConsole.Ask<int>(" -> Enter Seat Id: ");

            showSeat.Show.Id = AnsiConsole.Ask<int>(" -> Enter Show Id: ");

            showSeat.SeatStatus = AnsiConsole.Ask<SeatStatus>(" -> Enter SeatStatus: ");
            
            showSeat.Booking.Id = AnsiConsole.Ask<int>(" -> Enter Booking Id: ");


            Result result = _showSeatBUS.Create(showSeat);
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