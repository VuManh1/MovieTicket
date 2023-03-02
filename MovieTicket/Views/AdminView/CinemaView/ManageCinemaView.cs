using BUS;
using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.CinemaView
{
    public class ManageCinemaView : IViewRender
    {
		private readonly IViewFactory _viewFactory;

        public ManageCinemaView(IViewFactory viewFactory, CinemaBUS cinemaBUS)
		{
			_viewFactory = viewFactory;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Manage Cinema]");
        
			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Add Cinema", "Show All Cinemas", "Back"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			switch (selection)
			{
				case "Add Cinema":
                    _viewFactory.Render(ViewConstant.AddCinema);
					break;
                case "Show All Cinemas":
                    _viewFactory.Render(ViewConstant.AdminListCinema);
                    break;
				case "Back":
					_viewFactory.Render(ViewConstant.AdminHome);
					break;
			}
        }
    }
}