using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.GenreView
{
    public class ListGenreView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly GenreBUS _genreBUS;

        private const int GENRES_PER_PAGE = 10;

        public ListGenreView(IViewFactory viewFactory, GenreBUS genreBUS)
        {
            _viewFactory = viewFactory;
            _genreBUS = genreBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Genre> genres = _genreBUS.GetAll();

            if (genres.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)genres.Count / GENRES_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Genres by page
                List<Genre> genresToRender = genres.
                    Skip((page - 1) * GENRES_PER_PAGE)
                    .Take(GENRES_PER_PAGE).ToList();

                RenderGenres(genresToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No Genre :([/]");
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
                    _viewFactory.Render(ViewConstant.AdminListGenre, model: page - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.Render(ViewConstant.AdminListGenre, model: page + 1);
                    break;
                case ConsoleKey.F:
                    break;
            }
        }

        public void RenderGenres(List<Genre> genres)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Genre", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name");

            foreach (var genre in genres)
            {
                table.AddRow(
                    genre.Id.ToString(),
                    genre.Name
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
