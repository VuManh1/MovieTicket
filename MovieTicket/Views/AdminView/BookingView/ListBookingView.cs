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
        private readonly BookingBUS _bookingBUS;

        private const int BOOKINGS_PER_PAGE = 10;

        public ListBookingView(IViewFactory viewFactory, BookingBUS bookingBUS)
        {
            _viewFactory = viewFactory;
            _bookingBUS = bookingBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Booking> bookings = _bookingBUS.GetAll();

            if (bookings.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)bookings.Count / BOOKINGS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Bookings by page
                List<Booking> bookingsToRender = bookings.
                    Skip((page - 1) * BOOKINGS_PER_PAGE)
                    .Take(BOOKINGS_PER_PAGE).ToList();

                RenderBookings(bookingsToRender);

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

        public void RenderBookings(List<Booking> bookings)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Booking", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "User ID", "Show ID", "SeatCount","CreateTime","Total");

            foreach (var Booking in bookings)
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
