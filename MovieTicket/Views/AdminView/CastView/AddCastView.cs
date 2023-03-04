using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.CastView
{
    public class AddCastView : IViewRender
    {
		private readonly CastBUS _castBUS;
        private readonly IViewFactory _viewFactory;

        public AddCastView(CastBUS castBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _castBUS = castBUS;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AddCast;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Cast \n[/]");

            Cast cast = new()
            {
                Name = AnsiConsole.Ask<string>(" -> Enter Cast's name: "),
                About = AnsiConsole.Ask<string>(" -> Enter Cast's About (0 to skip): ")
            };
            if (cast.About == "0") cast.About = null;

            Result result = _castBUS.Create(cast);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add Cast successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.GetService(ViewConstant.ManageCast)?.Render();
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.GetService(ViewConstant.ManageCast)?.Render();
                    return;
                }

                _viewFactory.GetService(ViewConstant.AddCast)?.Render();
            }
        }
    }
}