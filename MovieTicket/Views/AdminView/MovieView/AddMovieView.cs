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
        private readonly IViewFactory _viewFactory;

        public AddMovieView(MovieBUS movieBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _movieBUS = movieBUS;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
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

            string? genres = AnsiConsole.Ask<string>(" -> Enter genre id (separate by ',') or 0 to skip: ");
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
                GenreIdString = genres
            };

            Result result = _movieBUS.Create(movie);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add movie successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.Render(ViewConstant.ManageMovie);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.Render(ViewConstant.ManageMovie);
                    return;
                }

                _viewFactory.Render(ViewConstant.AddMovie);
            }
        }
    }
}