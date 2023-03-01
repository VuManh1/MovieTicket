using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.HallView
{
    public class ListHallView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly HallBus _HallBUS;

        private const int HallS_PER_PAGE = 10;

        public ListHallView(IViewFactory viewFactory, HallBus HallBUS)
        {
            _viewFactory = viewFactory;
            _HallBUS = HallBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Hall> Halls = _HallBUS.GetAllBus();

            if (Halls.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)Halls.Count / HallS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Halls by page
                List<Hall> HallsToRender = Halls.
                    Skip((page - 1) * HallS_PER_PAGE)
                    .Take(HallS_PER_PAGE).ToList();

                RenderHalls(HallsToRender);

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

        public void RenderHalls(List<Hall> Halls)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Hall", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name", "Cinema ID", "SeatCount");

            foreach (var Hall in Halls)
            {
                table.AddRow(
                    Hall.Id.ToString(),
                    Hall.Name,
                    Hall.Cinema.Id.ToString(),
                    Hall.SeatCount.ToString()
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
