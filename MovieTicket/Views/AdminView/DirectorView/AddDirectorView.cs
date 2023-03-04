using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.DirectorView
{
    public class AddDirectorView : IViewRender
    {
		private readonly DirectorBUS _directorBUS;
        private readonly IViewFactory _viewFactory;

        public AddDirectorView(DirectorBUS directorBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _directorBUS = directorBUS;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AddDirector;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Director \n[/]");

            Director director = new();

            director.Name = AnsiConsole.Ask<string>(" -> Enter Director's name: ");
            
            director.About = AnsiConsole.Ask<string>(" -> Enter Director's About (0 to skip): ");
            if (director.About == "0") director.About = null;

            Result result = _directorBUS.Create(director);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add Director successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.GetService(ViewConstant.ManageDirector)?.Render();
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.GetService(ViewConstant.ManageDirector)?.Render();
                    return;
                }

                _viewFactory.GetService(ViewConstant.AddDirector)?.Render();
            }
        }
    }
}