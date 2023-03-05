using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;
using MovieTicket.SignIn;

namespace MovieTicket.Views.MemberView.BookingView
{
    public class ListBookingView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly UserBUS _userBUS;
        private readonly BookingBUS _bookingBUS;

        private const int BOOKINGS_PER_PAGE = 10;

        public ListBookingView(IViewFactory viewFactory, BookingBUS bookingBUS, UserBUS userBUS)
        {
            _viewFactory = viewFactory;
            _bookingBUS = bookingBUS;
            _userBUS = userBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.ListBooking;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (!SignInManager.IsLogin || SignInManager.User == null)
            {
                _viewFactory.GetService(ViewConstant.Start)?.Render();
                return;
            }

            User member = SignInManager.User;

            SearchModel searchModel = model != null ? (SearchModel)model : new SearchModel() { Page = 1 };

            int page = searchModel.Page;
            if (page <= 0) page = 1;

            List<Booking> bookings = _bookingBUS.GetByUserId(member.Id);

            if (bookings.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)bookings.Count / BOOKINGS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get shows by page
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
                _viewFactory.GetService(ViewConstant.Paging)?.Render(pagingModel);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No booking :([/]\n");
            }

            AnsiConsole.MarkupLine(" * Press [dodgerblue2]'C'[/] to choose a booking, [red]'ESCAPE'[/] to go back.");

            var key = ConsoleHelper.InputKey(new List<ConsoleKey>()
                {
                    ConsoleKey.LeftArrow,
                    ConsoleKey.RightArrow,
                    ConsoleKey.C,
                    ConsoleKey.Escape
                });
            
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    _viewFactory.GetService(ViewConstant.ListBooking)?.Render(new SearchModel()
                    {
                        Page = page - 1
                    });
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.GetService(ViewConstant.ListBooking)?.Render(new SearchModel()
                    {
                        Page = page + 1
                    });
                    break;
                case ConsoleKey.C:
                    int id = AnsiConsole.Ask<int>(" -> Enter booking's id (0 to cancel): ");

                    if (id == 0)
                    {
                        _viewFactory.GetService(ViewConstant.ListBooking)?.Render(new SearchModel()
                        {
                            Page = page
                        });
                        return;
                    }

                    _viewFactory.GetService(ViewConstant.BookingDetail)?.Render(id);
                    break;
                case ConsoleKey.Escape:
                    _viewFactory.GetService(ViewConstant.MemberDetail)?.Render(member.Id);
                    break;
            }
        }

        public void RenderBookings(List<Booking> bookings)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "BOOKINGS", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Movie", "Cinema", "Start Time", "Total");

            foreach (var booking in bookings)
            {
                table.AddRow(
                    booking.Id.ToString(),
                    booking.Show.Movie.Name,
                    booking.Show.Hall.Cinema.Name,
                    booking.Show.StartTime.ToString("dd-MM-yyyy HH:mm:ss"),
                    booking.Total.ToString()
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
