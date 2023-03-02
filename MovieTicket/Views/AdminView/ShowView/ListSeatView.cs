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
        private readonly ShowBus _ShowBUS;

        private const int ShowS_PER_PAGE = 10;

        public ListShowView(IViewFactory viewFactory, ShowBus ShowBUS)
        {
            _viewFactory = viewFactory;
            _ShowBUS = ShowBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Show> Shows = _ShowBUS.GetAllBus();

            if (Shows.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)Shows.Count / ShowS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Shows by page
                List<Show> ShowsToRender = Shows.
                    Skip((page - 1) * ShowS_PER_PAGE)
                    .Take(ShowS_PER_PAGE).ToList();

                RenderShows(ShowsToRender);

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

        public void RenderShows(List<Show> Shows)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Show", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Hall ID", "Movie ID", "StartTime");

            foreach (var Show in Shows)
            {
                table.AddRow(
                    Show.Id.ToString(),
                    Show.Hall.Id.ToString(),
                    Show.Movie.Id.ToString(),
                    Show.StartTime.ToString()
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
