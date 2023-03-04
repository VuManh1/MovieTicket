using MovieTicket.Factory;
using MovieTicket.SignIn;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.AdminView
{
    public class HomeView : IViewRender
    {
		private readonly IViewFactory _viewFactory;

		public HomeView(IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AdminHome;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Admin Menu]");

			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Manage Movies", "Manage Shows", "Manage Cinemas",
                        "Manage Members", "Manage Casts", "Manage Directors", "Logout"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			// switch view
			switch (selection)
			{
				case "Manage Movies":
                    _viewFactory.GetService(ViewConstant.ManageMovie)?.Render();
					break;
				case "Manage Shows":
                    _viewFactory.GetService(ViewConstant.ManageShow)?.Render();
                    break;
                case "Manage Cinemas":
                    _viewFactory.GetService(ViewConstant.ManageCinema)?.Render();
                    break;
                case "Manage Members":
                    _viewFactory.GetService(ViewConstant.AdminListMember)?.Render();
                    break;
                case "Manage Casts":
                    _viewFactory.GetService(ViewConstant.ManageCast)?.Render();
                    break;
                case "Manage Directors":
                    _viewFactory.GetService(ViewConstant.ManageDirector)?.Render();
                    break;
                case "Logout":
					SignInManager.Logout();

					_viewFactory.GetService(ViewConstant.Start)?.Render();
					break;
			}
        }


    }
}