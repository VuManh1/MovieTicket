using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.HallView
{
    public class AddHallView : IViewRender
    {
		private readonly HallBus _HallBUS;
        private readonly IViewFactory _viewFactory;

        public AddHallView(HallBus HallBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _HallBUS = HallBUS;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            Hall Hall = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Hall \n[/]");

            Hall.Name = AnsiConsole.Ask<string>(" -> Enter Hall's name: ");

            Hall.Cinema.Id = AnsiConsole.Ask<int>(" -> Enter Cinema Id: ");

            Hall.SeatCount = AnsiConsole.Ask<int>(" -> Enter SeatCount: ");

            Result result = _HallBUS.AddBus(Hall);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add Hall successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.Render(ViewConstant.ManageHall);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.Render(ViewConstant.ManageHall);
                    return;
                }

                _viewFactory.Render(ViewConstant.AddHall);
            }
        }
    }
}