using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.CinemaView
{
    public class ListCinemaView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly CinemaBUS _cinemaBUS;

        private const int CINEMAS_PER_PAGE = 10;

        public ListCinemaView(IViewFactory viewFactory, CinemaBUS CinemaBUS)
        {
            _viewFactory = viewFactory;
            _cinemaBUS = CinemaBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AdminListCinema;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            SearchModel searchModel = model != null ? (SearchModel)model : new SearchModel() { Page = 1 };

            int page = searchModel.Page;
            if (page <= 0) page = 1;

            List<Cinema> cinemas;
            if (searchModel.SearchValue != null)
            {
                AnsiConsole.Markup($"[{ColorConstant.Info}]Search for '{searchModel.SearchValue}'[/]\n");
                cinemas = _cinemaBUS.Find($"cinemas.name like '%{searchModel.SearchValue}%'");
            }
            else
                cinemas = _cinemaBUS.GetAll();


            if (cinemas.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)cinemas.Count / CINEMAS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get movies by page
                List<Cinema> cinemaToRender = cinemas.
                    Skip((page - 1) * CINEMAS_PER_PAGE)
                    .Take(CINEMAS_PER_PAGE).ToList();

                RenderCinemas(cinemaToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No cinema :([/]\n");
            }

            AnsiConsole.MarkupLine(" * Press [dodgerblue2]'C'[/] to choose a cinema, [dodgerblue2]'F'[/] to search cinemas, " +
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
                    _viewFactory.GetService(ViewConstant.AdminListCinema)?.Render(new SearchModel()
                    {
                        Page = page - 1,
                        SearchValue = searchModel.SearchValue,
                    }, previousView);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.GetService(ViewConstant.AdminListCinema)?.Render(new SearchModel()
                    {
                        Page = page + 1,
                        SearchValue = searchModel.SearchValue
                    }, previousView);
                    break;
                case ConsoleKey.F:
                    searchModel.SearchValue = AnsiConsole.Ask<string>(" -> Enter cinema's name to search: ");

                    _viewFactory.GetService(ViewConstant.AdminListCinema)?.Render(new SearchModel()
                    {
                        Page = 1,
                        SearchValue = searchModel.SearchValue
                    }, ViewConstant.AdminListCinema);
                    break;
                case ConsoleKey.C:
                    int id = AnsiConsole.Ask<int>(" -> Enter cinema's id (0 to cancel): ");

                    if (id == 0)
                    {
                        _viewFactory.GetService(ViewConstant.AdminListCinema)?.Render(new SearchModel()
                        {
                            Page = page,
                            SearchValue = searchModel.SearchValue
                        }, previousView);
                        return;
                    }

                    _viewFactory.GetService(ViewConstant.AdminCinemaDetail)?.Render(id);
                    break;
                case ConsoleKey.Escape:
                    _viewFactory.GetService(previousView ?? ViewConstant.ManageCinema)?.Render();
                    break;
            }
        }

        public void RenderCinemas(List<Cinema> cinemas)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Cinemas", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name", "HallCount", "City Name");

            foreach (var cinema in cinemas)
            {
                table.AddRow(
                    cinema.Id.ToString(),
                    cinema.Name,
                    cinema.HallCount.ToString(),
                    cinema.City?.Name ?? ""
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
