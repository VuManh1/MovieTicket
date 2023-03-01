using MovieTicket.Factory;
using MovieTicket.Views;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.MovieView
{
    public class MovieManageView : IViewRender
    {
		private readonly IViewFactory _viewFactory;

		public MovieManageView(IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Manage Movie]");
        
			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Add Movie", "Show All Movies", "Back"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			switch (selection)
			{
				case "Add Movie":
                    _viewFactory.Render(ViewConstant.AddMovie);
					break;
                case "Show All Movies":
                    _viewFactory.Render(ViewConstant.AdminListMovie);
                    break;
				case "Back":
					_viewFactory.Render(ViewConstant.AdminHome);
					break;
			}
        }
    }
}