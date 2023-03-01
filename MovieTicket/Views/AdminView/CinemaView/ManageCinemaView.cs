using MovieTicket.Factory;
using MovieTicket.Views;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.CinemaView
{
    public class CinemaManageView : IViewRender
    {
		private readonly IViewFactory _viewFactory;

		public CinemaManageView(IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

        public void Render(string? statusMessage = null, object? model = null)
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