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
        private readonly MovieBUS _movieBUS;

        private const int MOVIES_PER_PAGE = 10;

        public ListMovieView(IViewFactory viewFactory, MovieBUS movieBUS)
        {
            _viewFactory = viewFactory;
            _movieBUS = movieBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AdminListMovie;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            SearchModel searchModel = model != null ? (SearchModel)model : new SearchModel() { Page = 1 };

            int page = searchModel.Page;
            if (page <= 0) page = 1;

            List<Movie> movies;
            if (searchModel.SearchValue != null) 
            {
                AnsiConsole.Markup($"[{ColorConstant.Success}]Search for '{searchModel.SearchValue}'[/]\n");
                movies = _movieBUS.Find($"NormalizeName like '%{searchModel.SearchValue}%'");
            }
            else
                movies = _movieBUS.GetAll();


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
                _viewFactory.GetService(ViewConstant.Paging)?.Render(pagingModel);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No movie :([/]\n");
            }

            AnsiConsole.MarkupLine(" * Press [dodgerblue2]'C'[/] to choose a movie, [dodgerblue2]'F'[/] to search movies, " +
                "[red]'ESCAPE'[/] to go back");
            var key = ConsoleHelper.InputKey(new List<ConsoleKey>()
                {
                    ConsoleKey.LeftArrow,
                    ConsoleKey.RightArrow,
                    ConsoleKey.F,
                    ConsoleKey.C,
                    ConsoleKey.Escape
                });
            
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    _viewFactory.GetService(ViewConstant.AdminListMovie)?.Render(new SearchModel()
                    {
                        Page = page - 1,
                        SearchValue = searchModel.SearchValue,
                    }, previousView);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.GetService(ViewConstant.AdminListMovie)?.Render(new SearchModel()
                    {
                        Page = page + 1,
                        SearchValue = searchModel.SearchValue
                    }, previousView);
                    break;
                case ConsoleKey.F:
                    searchModel.SearchValue = AnsiConsole.Ask<string>(" -> Enter movie's name to search: ");

                    _viewFactory.GetService(ViewConstant.AdminListMovie)?.Render(new SearchModel()
                    {
                        Page = 1,
                        SearchValue = searchModel.SearchValue
                    }, ViewConstant.AdminListMovie);
                    break;
                case ConsoleKey.C:
                    int id = AnsiConsole.Ask<int>(" -> Enter movie's id (0 to cancel): ");

                    if (id == 0)
                    {
                        _viewFactory.GetService(ViewConstant.AdminListMovie)?.Render(new SearchModel()
                        {
                            Page = page,
                            SearchValue = searchModel.SearchValue
                        }, previousView);
                        return;
                    }

                    _viewFactory.GetService(ViewConstant.AdminMovieDetail)?.Render(id);
                    break;
                case ConsoleKey.Escape:
                    _viewFactory.GetService(previousView ?? ViewConstant.ManageMovie)?.Render();
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
