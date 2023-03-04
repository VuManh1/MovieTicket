using BUS;
using MovieTicket.Factory;
using MovieTicket.SignIn;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Helpers;
using Spectre.Console;

namespace MovieTicket.Views.MemberView.MovieView
{
    public class MovieDetailView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly MovieBUS _movieBUS;

        public MovieDetailView(IViewFactory viewFactory, MovieBUS movieBUS)
        {
            _viewFactory = viewFactory;
            _movieBUS = movieBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.MovieDetail;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (model == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("movie", ViewConstant.MemberHome);
                return;
            }

            Movie? movie = _movieBUS.GetById((int)model);

            if (movie == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("movie", ViewConstant.MemberHome);
                return;
            }

            RenderMovieDetail(movie);

            List<ConsoleKey> keys = SignInManager.IsLogin && movie.MovieStatus == MovieStatus.Playing ?
                new List<ConsoleKey>()
                {
                    ConsoleKey.Enter,
                    ConsoleKey.Escape
                } :
                new List<ConsoleKey>() 
                {
                    ConsoleKey.Escape
                };

            var key = ConsoleHelper.InputKey(keys);

            switch (key)
            {
                case ConsoleKey.Enter:
                    _viewFactory.GetService(ViewConstant.SelectShow)?.Render(movie.Id);
                    break;
                case ConsoleKey.Escape:
                    _viewFactory.GetService(ViewConstant.MemberHome)?.Render();
                    break;
            }
        }

        public void RenderMovieDetail(Movie movie)
        {
            // create layout
            var layout = new Layout("Root")
                .SplitRows(
                    new Layout("Top").SplitColumns(
                        new Layout("Left"),
                        new Layout("Right")
                    ),
                    new Layout("Bottom"));

            // get genres, casts and directors from movie
            List<Genre> genres = _movieBUS.GetGenres(movie);
            List<Cast> casts = _movieBUS.GetCasts(movie);
            List<Director> directors = _movieBUS.GetDirectors(movie);

            Rows leftRows = new(
                new Markup($"[{ColorConstant.Primary}]{movie.Name.ToUpper()}[/]\n").Centered(),
                new Markup($"[{ColorConstant.Primary}]Length: [/]{movie.Length} minutes"),
                new Markup($"[{ColorConstant.Primary}]Release Date: [/]{movie.ReleaseDate}"),
                new Markup($"[{ColorConstant.Primary}]Country: [/]{movie.Country}"),
                new Markup($"[{ColorConstant.Primary}]Status: [/]{movie.MovieStatus}"),
                new Markup($"[{ColorConstant.Primary}]Genres: [/]{String.Join(", ", genres.Select(g => g.Name))}"),
                new Markup($"[{ColorConstant.Primary}]Casts: [/]{String.Join(", ", casts.Select(c => c.Name))}"),
                new Markup($"[{ColorConstant.Primary}]Directors: [/]{String.Join(", ", directors.Select(d => d.Name))}")
            );

            layout["Left"].Ratio = 1;
            layout["Left"].Update(
                new Panel(
                    Align.Left(leftRows, VerticalAlignment.Middle)
                )
                {
                    Border = BoxBorder.Heavy,
                    BorderStyle = new Style(Color.PaleGreen3)
                }
            );

            Rows rightRows = new(
                new Markup($"[{ColorConstant.Primary}]DESCRIPTION: [/]\n"),
                new Text(movie.Description ?? "null")
            );

            layout["Right"].Ratio = 2;
            layout["Right"].Update(
                new Panel(
                    Align.Center(rightRows, VerticalAlignment.Middle)
                )
                {
                    Border = BoxBorder.Heavy,
                    BorderStyle = new Style(Color.PaleGreen3)
                }
            );

            // update bottom
            if (movie.MovieStatus == MovieStatus.Playing)
            {
                if (SignInManager.IsLogin)
                {
                    layout["Bottom"].Update(
                        new Markup(" * Press [PaleGreen3]'ENTER'[/] to book ticket, [red]'ESC'[/] to go back. \n")
                    );
                }
                else
                {
                    layout["Bottom"].Update(
                        new Markup($" * [{ColorConstant.Primary}]You must sign in to booking ticket[/], Press [red]'ESC'[/] to go back. \n")
                    );
                }
            }
            else if (movie.MovieStatus == MovieStatus.Coming)
            {
                layout["Bottom"].Update(
                    new Markup($" * [{ColorConstant.Primary}]This movie will be released on {movie.ReleaseDate.ToString()}[/]," +
                    $" Press [red]'ESC'[/] to go back. \n")
                );
            }
            else
            {
                layout["Bottom"].Update(
                    new Markup($" * [{ColorConstant.Primary}]Watch this movie online on[/]" +
                    $" [link]https://phimstrong.net[/]," +
                    $" Press [red]'ESC'[/] to go back. \n")
                );
            }
            AnsiConsole.Write(layout);
        }
    }
}