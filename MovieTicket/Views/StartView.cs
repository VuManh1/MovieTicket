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
            Console.Clear();
            Console.Title = ViewConstant.Start;

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
                    _viewFactory.GetService(ViewConstant.MemberHome)?.Render();
                    break;
                case "Login":
					_viewFactory.GetService(ViewConstant.Login)?.Render();
					break;
				case "Register":
					_viewFactory.GetService(ViewConstant.Register)?.Render();
					break;
				case "Exit":
					AnsiConsole.MarkupLine("[PaleGreen3]Goodbye ![/]");
					break;
			}
		}
	}
}
