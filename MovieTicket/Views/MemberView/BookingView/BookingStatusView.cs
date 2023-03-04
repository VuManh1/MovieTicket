using MovieTicket.Factory;
using MovieTicket.SignIn;
using SharedLibrary.Constants;
using SharedLibrary.Helpers;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.MemberView.BookingView
{
    public class BookingStatusView : IViewRender
    {
        private readonly IViewFactory _viewFactory;

        public BookingStatusView(IViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.BookingStatus;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (!SignInManager.IsLogin)
            {
                _viewFactory.GetService(ViewConstant.Start)?.Render();
                return;
            }

            if (model == null || statusMessage == null)
            {
                _viewFactory.GetService(ViewConstant.MemberHome)?.Render();
                return;
            }

            BookingModel bookingModel = (BookingModel)model;

            ConsoleKey key;
            if (statusMessage.StartsWith("Success"))
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Successful booking for movie:[/] {bookingModel.Show.Movie.Name}");
                AnsiConsole.MarkupLine($" * Press [dodgerblue2]'V'[/] to view booking detail, " +
                    $"[{ColorConstant.Primary}]'ENTER'[/] to go back home page.");

                key = ConsoleHelper.InputKey(new List<ConsoleKey>()
                    {
                        ConsoleKey.Enter,
                        ConsoleKey.V
                    });
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Booking failed :((, try again.[/]");
                AnsiConsole.MarkupLine($" * Press [dodgerblue2]'B'[/] to go back, " +
                    $"[{ColorConstant.Primary}]'ENTER'[/] to go back home page.");

                key = ConsoleHelper.InputKey(new List<ConsoleKey>()
                    {
                        ConsoleKey.Enter,
                        ConsoleKey.B
                    });
            }

            switch (key)
            {
                case ConsoleKey.Enter:
                    _viewFactory.GetService(ViewConstant.MemberHome)?.Render();
                    break;
                case ConsoleKey.B:
                    _viewFactory.GetService(ViewConstant.ConfirmBooking)?.Render(bookingModel);
                    break;
                case ConsoleKey.V:
                    _viewFactory.GetService(ViewConstant.BookingDetail)?.Render(bookingModel.Booking.Id, ViewConstant.MemberHome);
                    break;
            }
        }
    }
}
