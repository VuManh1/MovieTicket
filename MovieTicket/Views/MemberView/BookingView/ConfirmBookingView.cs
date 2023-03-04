using BUS;
using MovieTicket.Factory;
using MovieTicket.SignIn;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Helpers;
using SharedLibrary.Models;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace MovieTicket.Views.MemberView.BookingView
{
    public class ConfirmBookingView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly BookingBUS _bookingBUS;

        public ConfirmBookingView(IViewFactory viewFactory, BookingBUS bookingBUS)
        {
            _viewFactory = viewFactory;
            _bookingBUS = bookingBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.ConfirmBooking;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (!SignInManager.IsLogin || SignInManager.User == null)
            {
                _viewFactory.GetService(ViewConstant.Start)?.Render();
                return;
            }

            if (model == null)
            {
                _viewFactory.GetService(ViewConstant.MemberHome)?.Render();
                return;
            }

            BookingModel bookingModel = (BookingModel)model;

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Confirm booking for movie:[/] {bookingModel.Show.Movie.Name}");

            RenderBookingDetail(bookingModel.Show, bookingModel.Seats);

            var key = ConsoleHelper.InputKey(new List<ConsoleKey>()
                    {
                        ConsoleKey.C,
                        ConsoleKey.Escape
                    });

            switch (key)
            {
                case ConsoleKey.Escape:
                    _viewFactory.GetService(ViewConstant.SelectSeat)?.Render(bookingModel.Show.Id);
                    return;
                case ConsoleKey.C:
                    double total = 0;
                    bookingModel.Seats.ForEach(s => total += s.Price);

                    Booking booking = new()
                    {
                        User = SignInManager.User,
                        Show = bookingModel.Show,
                        CreateTime = DateTime.Now,
                        SeatCount = bookingModel.Seats.Count,
                        Total = total
                    };

                    Result result = _bookingBUS.Create(booking, bookingModel.Seats);
                    bookingModel.Booking = booking;

                    if (result.Success)
                    {
                        _viewFactory.GetService(ViewConstant.BookingStatus)?.Render(bookingModel, statusMessage: "Success");
                        return;
                    }
                    else
                    {
                        Console.WriteLine(result.Message);
                        Console.ReadKey();
                        _viewFactory.GetService(ViewConstant.BookingStatus)?.Render(bookingModel, statusMessage: "Error");
                        return;
                    }
            }
        }

        public void RenderBookingDetail(Show show, List<Seat> seats)
        {
            // create layout
            var layout = new Layout("Root")
                .SplitRows(
                    new Layout("Top").SplitColumns(
                        new Layout("Left"),
                        new Layout("Right")
                    ),
                    new Layout("Bottom"));

            Rows leftRows = new(
                new Markup($"[{ColorConstant.Primary}]{show.Movie.Name.ToUpper()}[/]\n").Centered(),
                new Markup($"[{ColorConstant.Primary}]Length: [/]{show.Movie.Length} minutes"),
                new Markup($"[{ColorConstant.Primary}]Country: [/]{show.Movie.Country}"),
                new Markup($"[{ColorConstant.Primary}]Release Date: [/]{show.Movie.ReleaseDate}")
            );

            layout["Left"].Ratio = 1;
            layout["Left"].Update(
                new Panel(
                    Align.Left(leftRows, VerticalAlignment.Middle)
                )
                {
                    Border = BoxBorder.Heavy,
                    BorderStyle = new Style(Color.PaleGreen3)
                }
            );

            double total = 0;
            seats.ForEach(s => total += s.Price);

            Rows rightRows = new(
                new Markup($"[{ColorConstant.Primary}]Booking Information: [/]\n").Centered(),
                new Markup($"[{ColorConstant.Primary}]Start Time: [/]{show.StartTime}"),
                new Markup($"[{ColorConstant.Primary}]Cinema: [/]{show.Hall.Cinema.Name}"),
                new Markup($"[{ColorConstant.Primary}]Hall: [/]{show.Hall.Name}"),
                new Markup($"[{ColorConstant.Primary}]Seats: [/]{String.Join(", ", seats.Select(s => s.SeatName))}\n"),
                new Markup($"[{ColorConstant.Primary}]Total: [/]{total}\n")
            );

            layout["Right"].Ratio = 2;
            layout["Right"].Update(
                new Panel(
                    Align.Left(rightRows, VerticalAlignment.Middle)
                )
                {
                    Border = BoxBorder.Heavy,
                    BorderStyle = new Style(Color.PaleGreen3)
                }
            );

            layout["Bottom"].Update(
                new Markup(" * Press [dodgerblue2]'C'[/] to [palegreen3]confirm booking[/], [red]'ESC'[/] to go back.")
            );

            AnsiConsole.Write(layout);
        }
    }
}
