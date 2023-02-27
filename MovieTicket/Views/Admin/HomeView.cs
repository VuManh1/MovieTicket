using MovieTicket.Factory;
using MovieTicket.SignIn;
using Spectre.Console;

namespace MovieTicket.Views.Admin
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
			// create panel
			var panel = new Panel(
				Align.Center(
                    new Rows(
                        new FigletText("MOVIE TICKET")
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


            panel.Header = new PanelHeader(SignInManager.User?.Name ?? "Not login");
            panel.Header.RightJustified<PanelHeader>();
        
			AnsiConsole.Write(panel);

			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Manage Movies", "Manage Shows", "Manage Cinemas",
                        "Logout"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			// switch view
			switch (selection)
			{
				case "Manage Movies":
                    _viewFactory.Render("AdminMovieMenu");
					break;
				case "Manage Shows":
					break;
				case "Logout":
					AnsiConsole.MarkupLine("[PaleGreen3]Goodbye ![/]");
					break;
			}
        }


    }
}