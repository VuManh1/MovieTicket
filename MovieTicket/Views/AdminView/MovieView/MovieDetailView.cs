using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.MovieView
{
    public class MovieDetailView : IViewRender
    {
		private readonly MovieBUS _movieBUS;
        private readonly GenreBUS _genreBUS;
        private readonly IViewFactory _viewFactory;

        public MovieDetailView(MovieBUS movieBUS, GenreBUS genreBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _movieBUS = movieBUS;
            _genreBUS = genreBUS;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (model == null)
            {
                _viewFactory.Render(ViewConstant.NotFound, "movie", ViewConstant.AdminListMovie);
                return;
            }

            int movieid = (int)model;

            Movie? movie = _movieBUS.GetById(movieid);

            if (movie == null)
            {
                _viewFactory.Render(ViewConstant.NotFound, "movie", ViewConstant.AdminListMovie);
                return;
            }

            // render movie detail
            RenderMovie(movie);

            // check status message
            if (statusMessage != null)
            {
                if (statusMessage.StartsWith("Success"))
                    AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Save changes successful ![/]\n");
                else if (statusMessage.StartsWith("Error"))
                    AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{statusMessage}[/]\n");
            }

            // create select: 
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a action: ")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Go Back", "Delete this movie",
                        "Change Name", "Change Description", "Change Length", "Change Release Date",
                        "Change Country", "Change Status", "Change Casts", "Change Directors", "Change Genres"
                    })
                    .HighlightStyle(new Style(Color.PaleGreen3)));


            switch (selection)
            {
                case "Go Back":
                    _viewFactory.Render(ViewConstant.AdminListMovie);
                    return;
                case "Delete this movie":
                    if (!AnsiConsole.Confirm("Delete this movie ? : "))
                    {
                        _viewFactory.Render(ViewConstant.AdminMovieDetail, movie.Id);
                        return;
                    }

                    Result deleteResult = _movieBUS.Delete(movie);

                    if (deleteResult.Success)
                        _viewFactory.Render(ViewConstant.AdminListMovie);
                    else
                        _viewFactory.Render(ViewConstant.AdminMovieDetail, movie.Id, statusMessage: "Error !, " + deleteResult.Message);

                    return;
                case "Change Name":
                    movie.Name = AnsiConsole.Ask<string>(" -> Change movie's name: ");
                    break;
                case "Change Description":
                    movie.Description = AnsiConsole.Ask<string>(" -> Change movie's description: ");
                    break;
                case "Change Length":
                    movie.Length = AnsiConsole.Ask<int>(" -> Change movie's Length: ");
                    break;
                case "Change Release Date":
                    movie.ReleaseDate = DateOnly.FromDateTime(AnsiConsole.Ask<DateTime>(" -> Change movie's Release Date: "));
                    break;
                case "Change Country":
                    movie.Country = AnsiConsole.Ask<string>(" -> Change movie's Country: ");
                    break;
                case "Change Status":
                    movie.MovieStatus = AnsiConsole.Ask<MovieStatus>(" -> Change movie's Status: ");
                    break;
                case "Change Casts":
                    movie.CastIdString = AnsiConsole.Ask<string>(" -> Change movie's Casts (Enter id separate by ','): ");
                    break;
                case "Change Directors":
                    movie.DirectorIdString = AnsiConsole.Ask<string>(" -> Change movie's Directors (Enter id separate by ','): ");
                    break;
                case "Change Genres":
                    movie.GenreString = GetGenres();
                    break;
            }

            Result result = _movieBUS.Update(movie);

            if (result.Success)
                _viewFactory.Render(ViewConstant.AdminMovieDetail, movie.Id, statusMessage: "Successful change movie detail !");
            else
                _viewFactory.Render(ViewConstant.AdminMovieDetail, movie.Id, statusMessage: "Error !, " + result.Message);
        }

        public string? GetGenres()
        {
            List<string> genres = _genreBUS.GetAll().Select(g => g.Name).ToList();

            Console.WriteLine();
            var fruits = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Change [green]genres[/]: ")
                    .NotRequired()
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more genres)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a genre, " +
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(genres));

            return fruits.Count > 0 ? String.Join(",", fruits) : null;
        }

        public void RenderMovie(Movie movie)
        {
            List<Genre> genres = _movieBUS.GetGenres(movie);
            List<Cast> casts = _movieBUS.GetCasts(movie);
            List<Director> directors = _movieBUS.GetDirectors(movie);

            Rows rows = new(
                new Markup($"[{ColorConstant.Primary}]Id: [/]{movie.Id}"),
                new Markup($"[{ColorConstant.Primary}]Name: [/]{movie.Name}"),
                new Markup($"[{ColorConstant.Primary}]Description: [/]{movie.Description}"),
                new Markup($"[{ColorConstant.Primary}]Length: [/]{movie.Length}"),
                new Markup($"[{ColorConstant.Primary}]Release Date: [/]{movie.ReleaseDate}"),
                new Markup($"[{ColorConstant.Primary}]Country: [/]{movie.Country}"),
                new Markup($"[{ColorConstant.Primary}]Status: [/]{movie.MovieStatus}"),
                new Markup($"[{ColorConstant.Primary}]Genres: [/]{String.Join(", ", genres.Select(g => g.Name))}"),
                new Markup($"[{ColorConstant.Primary}]Casts: [/]{String.Join(", ", casts.Select(c => c.Name))}"),
                new Markup($"[{ColorConstant.Primary}]Directors: [/]{String.Join(", ", directors.Select(d => d.Name))}")
            );

            var panel = new Panel(
                Align.Left(rows))
            {
                Border = BoxBorder.Heavy,
                BorderStyle = new Style(Color.PaleGreen3),
                Expand = true,
                Header = new PanelHeader("Movie Detail")
            };
            panel.Header.Centered();

            AnsiConsole.Write(panel);
        }
    }
}