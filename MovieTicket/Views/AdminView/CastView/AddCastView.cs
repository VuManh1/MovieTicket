using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.CastView
{
    public class AddCastView : IViewRender
    {
		private readonly CastBus _CastBUS;
        private readonly IViewFactory _viewFactory;

        public AddCastView(CastBus CastBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _CastBUS = CastBUS;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            Cast cast = new Cast();
            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Cast \n[/]");

            cast.Name = AnsiConsole.Ask<string>(" -> Enter Cast's name: ");
            
            cast.About = AnsiConsole.Ask<string>(" -> Enter Cast's About (0 to skip): ");
            if (cast.About == "0") cast.About = null;


            Result result = _CastBUS.AddBus(cast);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add Cast successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.Render(ViewConstant.ManageCast);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.Render(ViewConstant.ManageCast);
                    return;
                }

                _viewFactory.Render(ViewConstant.AddCast);
            }
        }
    }
}