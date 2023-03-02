using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.ShowView
{
    public class ListShowView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly ShowBUS _showBUS;

        private const int SHOWS_PER_PAGE = 10;

        public ListShowView(IViewFactory viewFactory, ShowBUS showBUS)
        {
            _viewFactory = viewFactory;
            _showBUS = showBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Show> shows = _showBUS.GetAll();

            if (shows.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)shows.Count / SHOWS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Shows by page
                List<Show> showsToRender = shows.
                    Skip((page - 1) * SHOWS_PER_PAGE)
                    .Take(SHOWS_PER_PAGE).ToList();

                RenderShows(showsToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No Show :([/]");
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
                    _viewFactory.Render(ViewConstant.AdminListShow, model: page - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.Render(ViewConstant.AdminListShow, model: page + 1);
                    break;
                case ConsoleKey.F:
                    break;
            }
        }

        public void RenderShows(List<Show> shows)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Show", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Hall ID", "Movie ID", "StartTime");

            foreach (var show in shows)
            {
                table.AddRow(
                    show.Id.ToString(),
                    show.Hall.Id.ToString(),
                    show.Movie.Id.ToString(),
                    show.StartTime.ToString()
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
