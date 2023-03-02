using BUS;
using MovieTicket.Factory;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Helpers;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.HallView
{
    public class ListHallView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly HallBUS _hallBUS;

        private const int HALLS_PER_PAGE = 10;

        public ListHallView(IViewFactory viewFactory, HallBUS hallBUS)
        {
            _viewFactory = viewFactory;
            _hallBUS = hallBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Hall> halls = _hallBUS.GetAll();

            if (halls.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)halls.Count / HALLS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Halls by page
                List<Hall> hallsToRender = halls.
                    Skip((page - 1) * HALLS_PER_PAGE)
                    .Take(HALLS_PER_PAGE).ToList();

                RenderHalls(hallsToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No Hall :([/]");
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
                    _viewFactory.Render(ViewConstant.AdminListHall, model: page - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.Render(ViewConstant.AdminListHall, model: page + 1);
                    break;
                case ConsoleKey.F:
                    break;
            }
        }

        public void RenderHalls(List<Hall> halls)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Hall", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name", "Cinema ID", "SeatCount");

            foreach (var hall in halls)
            {
                table.AddRow(
                    hall.Id.ToString(),
                    hall.Name,
                    hall.Cinema.Id.ToString(),
                    hall.SeatCount.ToString()
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
