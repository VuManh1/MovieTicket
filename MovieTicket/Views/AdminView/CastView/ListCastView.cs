using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.CastView
{
    public class ListCastView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly CastBUS _castBUS;

        private const int CASTS_PER_PAGE = 10;

        public ListCastView(IViewFactory viewFactory, CastBUS castBUS)
        {
            _viewFactory = viewFactory;
            _castBUS = castBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AdminListCast;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            SearchModel searchModel = model != null ? (SearchModel)model : new SearchModel() { Page = 1 };

            int page = searchModel.Page;
            if (page <= 0) page = 1;

            List<Cast> casts;
            if (searchModel.SearchValue != null)
            {
                AnsiConsole.Markup($"[{ColorConstant.Success}]Search for '{searchModel.SearchValue}'[/]\n");
                casts = _castBUS.Find(searchModel.SearchValue);
            }
            else
                casts = _castBUS.GetAll();


            if (casts.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)casts.Count / CASTS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get movies by page
                List<Cast> castsToRender = casts.
                    Skip((page - 1) * CASTS_PER_PAGE)
                    .Take(CASTS_PER_PAGE).ToList();

                RenderCasts(castsToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No cast :([/]\n");
            }

            AnsiConsole.MarkupLine(" * Press [dodgerblue2]'C'[/] to choose a cast, [dodgerblue2]'F'[/] to search casts, " +
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
                    _viewFactory.GetService(ViewConstant.AdminListCast)?.Render(new SearchModel()
                    {
                        Page = page - 1,
                        SearchValue = searchModel.SearchValue,
                    }, previousView);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.GetService(ViewConstant.AdminListCast)?.Render(new SearchModel()
                    {
                        Page = page + 1,
                        SearchValue = searchModel.SearchValue
                    }, previousView);
                    break;
                case ConsoleKey.F:
                    searchModel.SearchValue = AnsiConsole.Ask<string>(" -> Enter cast's name to search: ");

                    _viewFactory.GetService(ViewConstant.AdminListCast)?.Render(new SearchModel()
                    {
                        Page = 1,
                        SearchValue = searchModel.SearchValue
                    }, ViewConstant.AdminListCast);
                    break;
                case ConsoleKey.C:
                    int id = AnsiConsole.Ask<int>(" -> Enter cast's id (0 to cancel): ");

                    if (id == 0)
                    {
                        _viewFactory.GetService(ViewConstant.AdminListCast)?.Render(new SearchModel()
                        {
                            Page = page,
                            SearchValue = searchModel.SearchValue
                        }, previousView);
                        return;
                    }

                    _viewFactory.GetService(ViewConstant.AdminCastDetail)?.Render(id);
                    break;
                case ConsoleKey.Escape:
                    _viewFactory.GetService(previousView ?? ViewConstant.ManageCast)?.Render();
                    break;
            }
        }

        public void RenderCasts(List<Cast> casts)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Cast", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name");

            foreach (var cast in casts)
            {
                table.AddRow(
                    cast.Id.ToString(),
                    cast.Name
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
