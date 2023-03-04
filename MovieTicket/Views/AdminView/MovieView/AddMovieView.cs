using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.MovieView
{
    public class AddMovieView : IViewRender
    {
		private readonly MovieBUS _movieBUS;
		private readonly GenreBUS _genreBUS;
        private readonly IViewFactory _viewFactory;

        public AddMovieView(MovieBUS movieBUS, GenreBUS genreBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _movieBUS = movieBUS;
            _genreBUS = genreBUS;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AddMovie;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Movie \n[/]");

            string name = AnsiConsole.Ask<string>(" -> Enter movie's name: ");
            
            string? description = AnsiConsole.Ask<string>(" -> Enter movie's description (0 to skip): ");
            if (description == "0") description = null; 
                
            int length = AnsiConsole.Ask<int>(" -> Enter movie's length: ");
            
            DateOnly releaseDate = DateOnly.FromDateTime(
                AnsiConsole.Ask<DateTime>(" -> Enter movie's description ([yellow]EX: 12-20-2003[/]): "));

            MovieStatus status = AnsiConsole.Ask<MovieStatus>(
                " -> Enter movie's status ([green]'Playing'[/], [yellow]'Coming'[/], [red]'Stop'[/]): ");

            string country = AnsiConsole.Ask<string>(" -> Enter movie's country: ");

            string? genres = GetGenres();
            if (genres == "0") genres = null;

            string? casts = AnsiConsole.Ask<string>(" -> Enter cast id (separate by ',') or 0 to skip: ");
            if (casts == "0") casts = null;

            string? directors = AnsiConsole.Ask<string>(" -> Enter director id (separate by ',') or 0 to skip: ");
            if (directors == "0") directors = null;

            Movie movie = new()
            {
                Name = name,
                Description = description,
                Length = length,
                ReleaseDate = releaseDate,
                MovieStatus= status,
                Country = country,
                CastIdString = casts,
                DirectorIdString = directors,
                GenreString = genres
            };

            Result result = _movieBUS.Create(movie);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add movie successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.GetService(ViewConstant.ManageMovie)?.Render();
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.GetService(ViewConstant.ManageMovie)?.Render();
                    return;
                }

                _viewFactory.GetService(ViewConstant.AddMovie)?.Render();
            }
        }

        public string? GetGenres()
        {
            List<string> genres = _genreBUS.GetAll().Select(g => g.Name).ToList();

            Console.WriteLine();
            var fruits = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Select [green]genres[/]: ")
                    .NotRequired()
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more genres)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a genre, " +
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(genres));

            return fruits.Count > 0 ? String.Join(",", fruits) : null;
        }
    }
}