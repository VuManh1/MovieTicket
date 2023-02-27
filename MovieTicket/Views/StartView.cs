using BUS;
using MovieTicket.Factory;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views
{
	public class StartView : IViewRender
	{
		private readonly IViewFactory _viewFactory;
		private readonly MovieBus _movieBUS;

		public StartView(IViewFactory viewFactory, MovieBus movieBUS)
		{
			_viewFactory = viewFactory;
			_movieBUS = movieBUS;
		}

		public void Render(string? statusMessage = null, object? model = null)
		{
			List<Movie> movies = _movieBUS.GetAllBus();

			movies.ForEach(m => {
				Console.WriteLine($"{m.Name} {m.Description}");
			});

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
					_viewFactory.Render("login");
					break;
				case "Register":
					_viewFactory.Render("register");
					break;
				case "Exit":
					AnsiConsole.MarkupLine("[PaleGreen3]Goodbye ![/]");
					break;
			}
		}
	}
}
