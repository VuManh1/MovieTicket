using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.BookingView
{
    public class ListBookingView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly BookingBus _BookingBUS;

        private const int BookingS_PER_PAGE = 10;

        public ListBookingView(IViewFactory viewFactory, BookingBus BookingBUS)
        {
            _viewFactory = viewFactory;
            _BookingBUS = BookingBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Booking> Bookings = _BookingBUS.GetAllBus();

            if (Bookings.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)Bookings.Count / BookingS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Bookings by page
                List<Booking> BookingsToRender = Bookings.
                    Skip((page - 1) * BookingS_PER_PAGE)
                    .Take(BookingS_PER_PAGE).ToList();

                RenderBookings(BookingsToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No Booking :([/]");
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
                    _viewFactory.Render(ViewConstant.AdminListBooking, model: page - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.Render(ViewConstant.AdminListBooking, model: page + 1);
                    break;
                case ConsoleKey.F:
                    break;
            }
        }

        public void RenderBookings(List<Booking> Bookings)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Booking", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "User ID", "Show ID", "SeatCount","CreateTime","Total");

            foreach (var Booking in Bookings)
            {
                table.AddRow(
                    Booking.Id.ToString(),
                    Booking.User.Id.ToString(),
                    Booking.Show.Id.ToString(),
                    Booking.SeatCount.ToString(),
                    Booking.CreateTime.ToString(),
                    Booking.Total.ToString()
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
