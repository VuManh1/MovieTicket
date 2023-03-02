using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.ShowView
{
    public class AddShowView : IViewRender
    {
		private readonly ShowBUS _ShowBUS;
        private readonly IViewFactory _viewFactory;

        public AddShowView(ShowBUS ShowBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _ShowBUS = ShowBUS;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            Show Show = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Show \n[/]");
            
            Show.Hall.Id = AnsiConsole.Ask<int>(" -> Enter Hall Id: ");

            Show.Movie.Id = AnsiConsole.Ask<int>(" -> Enter Movie Id: ");

            Show.StartTime = AnsiConsole.Ask<DateTime>(" -> Enter Start Time: ");


            Result result = _ShowBUS.Create(Show);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add Show successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.Render(ViewConstant.ManageShow);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.Render(ViewConstant.ManageShow);
                    return;
                }

                _viewFactory.Render(ViewConstant.AddShow);
            }
        }
    }
}