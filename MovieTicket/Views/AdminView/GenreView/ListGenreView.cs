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
        private readonly GenreBus _GenreBUS;

        private const int GenreS_PER_PAGE = 10;

        public ListGenreView(IViewFactory viewFactory, GenreBus GenreBUS)
        {
            _viewFactory = viewFactory;
            _GenreBUS = GenreBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Genre> Genres = _GenreBUS.GetAllBus();

            if (Genres.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)Genres.Count / GenreS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Genres by page
                List<Genre> GenresToRender = Genres.
                    Skip((page - 1) * GenreS_PER_PAGE)
                    .Take(GenreS_PER_PAGE).ToList();

                RenderGenres(GenresToRender);

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

        public void RenderGenres(List<Genre> Genres)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Genre", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name");

            foreach (var Genre in Genres)
            {
                table.AddRow(
                    Genre.Id.ToString(),
                    Genre.Name
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
