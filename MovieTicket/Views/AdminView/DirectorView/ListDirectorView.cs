using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.DirectorView
{
    public class ListDirectorView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly DirectorBUS _directorBUS;

        private const int DIRECTORS_PER_PAGE = 10;

        public ListDirectorView(IViewFactory viewFactory, DirectorBUS directorBUS)
        {
            _viewFactory = viewFactory;
            _directorBUS = directorBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AdminListDirector;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            SearchModel searchModel = model != null ? (SearchModel)model : new SearchModel() { Page = 1 };

            int page = searchModel.Page;
            if (page <= 0) page = 1;

            List<Director> directors;
            if (searchModel.SearchValue != null)
            {
                AnsiConsole.Markup($"[{ColorConstant.Success}]Search for '{searchModel.SearchValue}'[/]\n");
                directors = _directorBUS.Find(searchModel.SearchValue);
            }
            else
                directors = _directorBUS.GetAll();


            if (directors.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)directors.Count / DIRECTORS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get movies by page
                List<Director> directorsToRender = directors.
                    Skip((page - 1) * DIRECTORS_PER_PAGE)
                    .Take(DIRECTORS_PER_PAGE).ToList();

                RenderDirectors(directorsToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No director :([/]\n");
            }

            AnsiConsole.MarkupLine(" * Press [dodgerblue2]'C'[/] to choose a director, [dodgerblue2]'F'[/] to search directors, " +
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
                    _viewFactory.GetService(ViewConstant.AdminListDirector)?.Render(new SearchModel()
                    {
                        Page = page - 1,
                        SearchValue = searchModel.SearchValue,
                    }, previousView);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.GetService(ViewConstant.AdminListDirector)?.Render(new SearchModel()
                    {
                        Page = page + 1,
                        SearchValue = searchModel.SearchValue
                    }, previousView);
                    break;
                case ConsoleKey.F:
                    searchModel.SearchValue = AnsiConsole.Ask<string>(" -> Enter director's name to search: ");

                    _viewFactory.GetService(ViewConstant.AdminListDirector)?.Render(new SearchModel()
                    {
                        Page = 1,
                        SearchValue = searchModel.SearchValue
                    }, ViewConstant.AdminListDirector);
                    break;
                case ConsoleKey.C:
                    int id = AnsiConsole.Ask<int>(" -> Enter director's id (0 to cancel): ");

                    if (id == 0)
                    {
                        _viewFactory.GetService(ViewConstant.AdminListDirector)?.Render(new SearchModel()
                        {
                            Page = page,
                            SearchValue = searchModel.SearchValue
                        }, previousView);
                        return;
                    }

                    _viewFactory.GetService(ViewConstant.AdminDirectorDetail)?.Render(id);
                    break;
                case ConsoleKey.Escape:
                    _viewFactory.GetService(previousView ?? ViewConstant.ManageDirector)?.Render();
                    break;
            }
        }

        public void RenderDirectors(List<Director> directors)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Director", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name");

            foreach (var director in directors)
            {
                table.AddRow(
                    director.Id.ToString(),
                    director.Name
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
