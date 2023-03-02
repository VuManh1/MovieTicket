using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.SeatView
{
    public class ListSeatView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly SeatBUS _seatBUS;

        private const int SEATS_PER_PAGE = 10;

        public ListSeatView(IViewFactory viewFactory, SeatBUS seatBUS)
        {
            _viewFactory = viewFactory;
            _seatBUS = seatBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Seat> seats = _seatBUS.GetAll();

            if (seats.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)seats.Count / SEATS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Seats by page
                List<Seat> seatsToRender = seats.
                    Skip((page - 1) * SEATS_PER_PAGE)
                    .Take(SEATS_PER_PAGE).ToList();

                RenderSeats(seatsToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No Seat :([/]");
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
                    _viewFactory.Render(ViewConstant.AdminListSeat, model: page - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.Render(ViewConstant.AdminListSeat, model: page + 1);
                    break;
                case ConsoleKey.F:
                    break;
            }
        }

        public void RenderSeats(List<Seat> seats)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Seat", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Hall ID", "SeatType", "SeatNumber","Price");

            foreach (var seat in seats)
            {
                table.AddRow(
                    seat.Id.ToString(),
                    seat.Hall.Id.ToString(),
                    seat.SeatType.ToString(),
                    seat.SeatNumber.ToString(),
                    seat.Price.ToString()
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
