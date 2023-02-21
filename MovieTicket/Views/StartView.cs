using MovieTicket.Factory;
using Spectre.Console;

namespace MovieTicket.Views
{
	public class StartView : IViewRender
	{
		private readonly IViewServiceFactory _viewServiceFactory;

		public StartView(IViewServiceFactory viewServiceFactory)
		{
			_viewServiceFactory = viewServiceFactory;
		}

		public void Render(string? statusMessage = null, object? model = null)
		{
			// create panel
			var panel = new Panel(
				Align.Center(
					new FigletText("NETFLOP")
						.LeftJustified()
						.Color(Color.Gold3_1),
					VerticalAlignment.Middle))
			{
				Border = BoxBorder.Heavy,
				BorderStyle = new Style(Color.PaleGreen3),
				Padding = new Padding(2, 2, 2, 2),
				Expand = true
			};

			AnsiConsole.Write(panel);

			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Login", "Register", "Exit"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			// switch view
			switch (selection)
			{
				case "Login":
					_viewServiceFactory.Render("login");
					break;
				case "Register":
					_viewServiceFactory.Render("register");
					break;
				case "Exit":
					AnsiConsole.MarkupLine("[PaleGreen3]Goodbye ![/]");
					break;
			}
		}
	}
}
