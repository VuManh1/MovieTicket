using MovieTicket.Factory;
using MovieTicket.Views;
using Spectre.Console;

namespace MovieTicket.Views.Admin.Movie
{
    public class MovieMenu : IViewRender
    {
		private readonly IViewFactory _viewFactory;

		public MovieMenu(IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
			// create panel
			var panel = new Panel(
				Align.Center(
                    new Rows(
                        new FigletText("MOVIE MENU")
						.LeftJustified()
						.Color(Color.Gold3_1),
                        new Text("[Admin Menu]")
                    )
				))
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
						"Add Movie", "Update Movie", "Show All Movies","Search Movie",
                        "Back"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			switch (selection)
			{
				case "Add Movie":
                    _viewFactory.Render("AdminMovieMenu");
					break;
				case "Update Movie":
					break;
                case "Show All Movies":
					break;
                case "Search Movie":
					break;
				case "Back":
					AnsiConsole.MarkupLine("[PaleGreen3]Goodbye ![/]");
					break;
			}
        }
    }
}