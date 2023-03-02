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
		private readonly GenreBUS _GenreBUS;
        private readonly IViewFactory _viewFactory;
        private readonly GenreBUS _GenreBus;

        public AddGenreView(GenreBUS GenreBUS, IViewFactory viewFactory,GenreBUS GenreBus)
		{
			_viewFactory = viewFactory;
            _GenreBUS = GenreBUS;
            _GenreBus = GenreBus;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            Genre Genre = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Genre \n[/]");

            Genre.Name = AnsiConsole.Ask<string>(" -> Enter Genre's name: ");

            Result result = _GenreBUS.Create(Genre);
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
			List<string> cities = _GenreBus.GetAll().Select(c => c.Name).ToList();
			cities.Insert(0, "Skip");

			Console.WriteLine();

			// create select Genre: 
			var Genre = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("\nChoose a Genre where you live: ")
					.PageSize(10)
					.AddChoices(cities)
					.HighlightStyle(new Style(Color.PaleGreen3)));

			return Genre;
		}
    }
}