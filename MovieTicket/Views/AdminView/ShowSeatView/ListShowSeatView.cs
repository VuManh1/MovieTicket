using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.ShowSeatView
{
    public class ListShowSeatView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly ShowSeatBus _ShowSeatBUS;

        private const int ShowSeatS_PER_PAGE = 10;

        public ListShowSeatView(IViewFactory viewFactory, ShowSeatBus ShowSeatBUS)
        {
            _viewFactory = viewFactory;
            _ShowSeatBUS = ShowSeatBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<ShowSeat> ShowSeats = _ShowSeatBUS.GetAllBus();

            if (ShowSeats.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)ShowSeats.Count / ShowSeatS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get ShowSeats by page
                List<ShowSeat> ShowSeatsToRender = ShowSeats.
                    Skip((page - 1) * ShowSeatS_PER_PAGE)
                    .Take(ShowSeatS_PER_PAGE).ToList();

                RenderShowSeats(ShowSeatsToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No ShowSeat :([/]");
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
                    _viewFactory.Render(ViewConstant.AdminListShowSeat, model: page - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.Render(ViewConstant.AdminListShowSeat, model: page + 1);
                    break;
                case ConsoleKey.F:
                    break;
            }
        }

        public void RenderShowSeats(List<ShowSeat> ShowSeats)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "ShowSeat", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Seat ID", "Show ID", "SeatStatus", "Booking ID");

            foreach (var ShowSeat in ShowSeats)
            {
                table.AddRow(
                    ShowSeat.Seat.Id.ToString(),
                    ShowSeat.Show.Id.ToString(),
                    ShowSeat.SeatStatus.ToString(),
                    ShowSeat.Booking.ToString() ?? ""
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
