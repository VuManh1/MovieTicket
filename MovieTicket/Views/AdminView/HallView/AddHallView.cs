using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.HallView
{
    public class AddHallView : IViewRender
    {
		private readonly HallBUS _hallBUS;
        private readonly IViewFactory _viewFactory;

        public AddHallView(HallBUS hallBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _hallBUS = hallBUS;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            Hall hall = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Hall \n[/]");

            hall.Name = AnsiConsole.Ask<string>(" -> Enter Hall's name: ");

            hall.Cinema.Id = AnsiConsole.Ask<int>(" -> Enter Cinema Id: ");

            hall.SeatCount = AnsiConsole.Ask<int>(" -> Enter SeatCount: ");

            Result result = _hallBUS.Create(hall);
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