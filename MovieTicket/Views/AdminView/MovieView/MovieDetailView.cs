using BUS;
using MovieTicket.Factory;
using MovieTicket.SignIn;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.MovieView
{
    public class MovieDetailView : IViewRender
    {
		private readonly MovieBus _movieBUS;
        private readonly IViewFactory _viewFactory;

        public MovieDetailView(MovieBus movieBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _movieBUS = movieBUS;
		}

        public void Render(string? statusMessage = null, object? model = null)
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
                        .Title("Choose a field you want to edit: ")
                        .PageSize(10)
                        .AddChoices(new[] {
                        "Go Back", "Delete this movie", 
                        "Name", "Description", "Length", "Release Date", 
                        "Country", "Status", "Casts", "Directors", "Genres"
                        })
                        .HighlightStyle(new Style(Color.PaleGreen3)));


            switch (selection)
            {
                case "Go Back":
                    _viewFactory.Render(ViewConstant.AdminListMovie);
                    return;
                case "Delete this movie":
                    Result deleteResult = _movieBUS.Delete(movie);

                    if (deleteResult.Success)
                        _viewFactory.Render(ViewConstant.AdminListMovie);
                    else
                        _viewFactory.Render(ViewConstant.AdminMovieDetail, "Error !, " + deleteResult.Message, movie.Id);

                    return;
                case "Name":
                    movie.Name = AnsiConsole.Ask<string>(" -> Change movie's name: ");
                    break;
                case "Description":
                    movie.Description = AnsiConsole.Ask<string>(" -> Change movie's description: ");
                    break;
                case "Length":
                    movie.Length = AnsiConsole.Ask<int>(" -> Change movie's Length: ");
                    break;
                case "Release Date":
                    movie.ReleaseDate = DateOnly.FromDateTime(AnsiConsole.Ask<DateTime>(" -> Change movie's Release Date: "));
                    break;
                case "Country":
                    movie.Country = AnsiConsole.Ask<string>(" -> Change movie's Country: ");
                    break;
                case "Status":
                    movie.MovieStatus = AnsiConsole.Ask<MovieStatus>(" -> Change movie's Status: ");
                    break;
                case "Casts":
                    movie.CastIdString = AnsiConsole.Ask<string>(" -> Change movie's Casts (Enter id separate by ','): ");
                    break;
                case "Directors":
                    movie.DirectorIdString = AnsiConsole.Ask<string>(" -> Change movie's Directors (Enter id separate by ','): ");
                    break;
                case "Genres":
                    movie.GenreIdString = AnsiConsole.Ask<string>(" -> Change movie's genres (Enter id separate by ','): ");
                    break;
            }

            Result result = _movieBUS.Update(movie);

            if (result.Success)
                _viewFactory.Render(ViewConstant.AdminMovieDetail, "Successful change movie detail !", movie.Id);
            else
                _viewFactory.Render(ViewConstant.AdminMovieDetail, "Error !, " + result.Message, movie.Id);
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