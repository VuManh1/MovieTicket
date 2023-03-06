using BUS;
using MovieTicket.Factory;
using MovieTicket.SignIn;
using OtpNet;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.MemberView.BookingView
{
    public class BookingDetailView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly BookingBUS _bookingBUS;

        public BookingDetailView(IViewFactory viewFactory, BookingBUS bookingBUS)
        {
            _viewFactory = viewFactory;
            _bookingBUS = bookingBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.BookingDetail;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (!SignInManager.IsLogin || SignInManager.User == null)
            {
                _viewFactory.GetService(ViewConstant.Start)?.Render();
                return;
            }

            User member = SignInManager.User;

            Booking? booking = _bookingBUS.GetById((int)(model ?? 0));

            if (booking == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("Booking", ViewConstant.MemberHome);
                return;
            }

            if (booking.User.Id != member.Id)
            {
                _viewFactory.GetService(ViewConstant.MemberHome)?.Render();
                return;
            }

            RenderBookingDetail(booking);

            // check status message
            if (statusMessage != null)
            {
                if (statusMessage.StartsWith("Error"))
                    AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{statusMessage}[/]\n");
            }

            // create select: 
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a action: ")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Go Back", "Delete this booking"
                    })
                    .HighlightStyle(new Style(Color.PaleGreen3)));

            switch (selection)
            {
                case "Go Back":
                    _viewFactory.GetService(previousView ?? ViewConstant.ListBooking)?.Render();
                    break;
                case "Delete this booking":
                    if (!AnsiConsole.Confirm("Delete this booking ? : "))
                    {
                        _viewFactory.GetService(ViewConstant.BookingDetail)?.Render(model, previousView);
                        return;
                    }

                    Result result = _bookingBUS.Delete(booking);
                    if (result.Success)
                    {
                        _viewFactory.GetService(ViewConstant.ListBooking)?.Render();
                    }
                    else
                    {
                        _viewFactory.GetService(ViewConstant.BookingDetail)?.Render(model, previousView, "Error !, can delete this booking");
                    }

                    break;
            }
        }

        public void RenderBookingDetail(Booking booking)
        {
            List<Seat> seats = _bookingBUS.GetShowSeats(booking).Select(b => b.Seat).ToList();

            Rows rows = new(
                new Markup($"[{ColorConstant.Primary}]Booking Information: [/]\n").Centered(),
                new Markup($"[{ColorConstant.Primary}]Movie: [/]{booking.Show.Movie.Name}"),
                new Markup($"[{ColorConstant.Primary}]Start Time: [/]{booking.Show.StartTime}"),
                new Markup($"[{ColorConstant.Primary}]Cinema: [/]{booking.Show.Hall.Cinema.Name}"),
                new Markup($"[{ColorConstant.Primary}]Hall: [/]{booking.Show.Hall.Name}"),
                new Markup($"[{ColorConstant.Primary}]Number of seat: [/]{booking.SeatCount}"),
                new Markup($"[{ColorConstant.Primary}]Seats: [/]{String.Join(", ", seats.Select(s => s.SeatName))}\n"),
                new Markup($"[{ColorConstant.Primary}]Total: [/]{booking.Total}\n")
            );

            var panel = new Panel(
                Align.Left(rows))
            {
                Border = BoxBorder.Heavy,
                BorderStyle = new Style(Color.PaleGreen3),
                Expand = true,
                Header = new PanelHeader("Booking Detail")
            };
            panel.Header.Centered();

            AnsiConsole.Write(panel);
        }
    }
}
