using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.MovieView
{
    public class ListMovieView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly MovieBus _movieBUS;

        private const int MOVIES_PER_PAGE = 10;

        public ListMovieView(IViewFactory viewFactory, MovieBus movieBUS)
        {
            _viewFactory = viewFactory;
            _movieBUS = movieBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Movie> movies = _movieBUS.GetAll();

            if (movies.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)movies.Count / MOVIES_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get movies by page
                List<Movie> moviesToRender = movies.
                    Skip((page - 1) * MOVIES_PER_PAGE)
                    .Take(MOVIES_PER_PAGE).ToList();

                RenderMovies(moviesToRender);

                PagingModel pagingModel = new()
                {
                    CurrentPage = page,
                    NumberOfPage = numberOfPage
                };

                // render pagination
                _viewFactory.GetService(ViewConstant.Paging)?.Render(model: pagingModel);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No movie :([/]");
            }

            AnsiConsole.MarkupLine("");
            var key = ConsoleHelper.InputKey(new List<ConsoleKey>()
                {
                    ConsoleKey.LeftArrow,
                    ConsoleKey.RightArrow
                });
            
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    _viewFactory.Render(ViewConstant.AdminListMovie, model: page - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.Render(ViewConstant.AdminListMovie, model: page + 1);
                    break;
                case ConsoleKey.F:
                    break;
            }
        }

        public void RenderMovies(List<Movie> movies)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "MOVIES", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name", "Country", "Status");

            foreach (var movie in movies)
            {
                table.AddRow(
                    movie.Id.ToString(),
                    movie.Name,
                    movie.Country ?? "",
                    movie.MovieStatus.ToString()
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
