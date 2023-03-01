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
        private readonly DirectorBus _DirectorBUS;

        private const int DirectorS_PER_PAGE = 10;

        public ListDirectorView(IViewFactory viewFactory, DirectorBus DirectorBUS)
        {
            _viewFactory = viewFactory;
            _DirectorBUS = DirectorBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Director> Directors = _DirectorBUS.GetAllBus();

            if (Directors.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)Directors.Count / DirectorS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Directors by page
                List<Director> DirectorsToRender = Directors.
                    Skip((page - 1) * DirectorS_PER_PAGE)
                    .Take(DirectorS_PER_PAGE).ToList();

                RenderDirectors(DirectorsToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No Director :([/]");
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
                    _viewFactory.Render(ViewConstant.AdminListDirector, model: page - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.Render(ViewConstant.AdminListDirector, model: page + 1);
                    break;
                case ConsoleKey.F:
                    break;
            }
        }

        public void RenderDirectors(List<Director> Directors)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Director", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name", "About");

            foreach (var Director in Directors)
            {
                table.AddRow(
                    Director.Id.ToString(),
                    Director.Name,
                    Director.About ?? ""
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
