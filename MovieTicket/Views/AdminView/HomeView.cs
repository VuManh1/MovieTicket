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

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Admin Menu]");

			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Manage Movies", "Manage Shows", "Manage Cinemas",
                        "Manage Members", "Logout"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			// switch view
			switch (selection)
			{
				case "Manage Movies":
                    _viewFactory.Render(ViewConstant.ManageMovie);
					break;
				case "Manage Shows":
                    _viewFactory.Render(ViewConstant.ManageShow);
                    break;
                case "Manage Cinemas":
                    _viewFactory.Render(ViewConstant.ManageCinema);
                    break;
                case "Manage Members":
                    _viewFactory.Render(ViewConstant.AdminListMember);
                    break;
                case "Logout":
					SignInManager.Logout();

					_viewFactory.Render(ViewConstant.Start);
					break;
			}
        }


    }
}