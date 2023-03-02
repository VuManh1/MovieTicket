using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.GenreView
{
    public class AddGenreView : IViewRender
    {
		private readonly GenreBUS _genreBUS;
        private readonly IViewFactory _viewFactory;

        public AddGenreView(GenreBUS genreBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _genreBUS = genreBUS;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            Genre genre = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Genre \n[/]");

            genre.Name = AnsiConsole.Ask<string>(" -> Enter Genre's name: ");

            Result result = _genreBUS.Create(genre);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add Genre successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.Render(ViewConstant.ManageGenre);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.Render(ViewConstant.ManageGenre);
                    return;
                }

                _viewFactory.Render(ViewConstant.AddGenre);
            }
        }

        public string GetGenre()
		{
			List<string> cities = _genreBUS.GetAll().Select(c => c.Name).ToList();
			cities.Insert(0, "Skip");

			Console.WriteLine();

			// create select Genre: 
			var genre = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("\nChoose a Genre where you live: ")
					.PageSize(10)
					.AddChoices(cities)
					.HighlightStyle(new Style(Color.PaleGreen3)));

			return genre;
		}
    }
}