using BUS;
using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views
{
	public class StartView : IViewRender
	{
		private readonly IViewFactory _viewFactory;

		public StartView(IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

		public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
		{
			_viewFactory.GetService(ViewConstant.Logo)?.Render();

			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Guest", "Login", "Register", "Exit"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			// switch view
			switch (selection)
			{
                case "Guest":

                    break;
                case "Login":
					_viewFactory.Render(ViewConstant.Login);
					break;
				case "Register":
					_viewFactory.Render(ViewConstant.Register);
					break;
				case "Exit":
					AnsiConsole.MarkupLine("[PaleGreen3]Goodbye ![/]");
					break;
			}
		}
	}
}
