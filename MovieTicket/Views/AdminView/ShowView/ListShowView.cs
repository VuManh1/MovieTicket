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
            Console.Clear();
            Console.Title = ViewConstant.AdminListShow;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            SearchModel searchModel = model != null ? (SearchModel)model : new SearchModel() { Page = 1 };

            int page = searchModel.Page;
            if (page <= 0) page = 1;

            List<Show> shows;
            if (searchModel.SearchValue != null) 
            {
                AnsiConsole.Markup($"[{ColorConstant.Success}]Search for '{searchModel.SearchValue}'[/]\n");
                shows = _showBUS.Find(searchModel.SearchValue);
            }
            else
                shows = _showBUS.GetAll();


            if (shows.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)shows.Count / SHOWS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get shows by page
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
                _viewFactory.GetService(ViewConstant.Paging)?.Render(pagingModel);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No show :([/]\n");
            }

            AnsiConsole.MarkupLine(" * Press [dodgerblue2]'C'[/] to choose a show, [dodgerblue2]'F'[/] to search shows, " +
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
                    _viewFactory.GetService(ViewConstant.AdminListShow)?.Render(new SearchModel()
                    {
                        Page = page - 1,
                        SearchValue = searchModel.SearchValue,
                    }, previousView);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.GetService(ViewConstant.AdminListShow)?.Render(new SearchModel()
                    {
                        Page = page + 1,
                        SearchValue = searchModel.SearchValue
                    }, previousView);
                    break;
                case ConsoleKey.F:
                    searchModel.SearchValue = AnsiConsole.Ask<string>(" -> Enter movie's name or cinema's name to search: ");

                    _viewFactory.GetService(ViewConstant.AdminListShow)?.Render(new SearchModel()
                    {
                        Page = 1,
                        SearchValue = searchModel.SearchValue
                    }, ViewConstant.AdminListShow);
                    break;
                case ConsoleKey.C:
                    int id = AnsiConsole.Ask<int>(" -> Enter show's id (0 to cancel): ");

                    if (id == 0)
                    {
                        _viewFactory.GetService(ViewConstant.AdminListShow)?.Render(new SearchModel()
                        {
                            Page = page,
                            SearchValue = searchModel.SearchValue
                        }, previousView);
                        return;
                    }

                    _viewFactory.GetService(ViewConstant.AdminShowDetail)?.Render(id);
                    break;
                case ConsoleKey.Escape:
                    _viewFactory.GetService(previousView ?? ViewConstant.ManageShow)?.Render();
                    break;
            }
        }

        public void RenderShows(List<Show> shows)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "SHOWS", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Movie", "Cinema", "City", "Start Time");

            foreach (var show in shows)
            {
                table.AddRow(
                    show.Id.ToString(),
                    show.Movie.Name,
                    show.Hall.Cinema.Name,
                    show.Hall.Cinema.City?.Name ?? "null",
                    show.StartTime.ToString("dd-MM-yyyy HH:mm:ss")
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
