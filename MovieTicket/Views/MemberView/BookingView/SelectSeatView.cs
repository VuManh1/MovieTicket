using BUS;
using MovieTicket.Factory;
using MovieTicket.SignIn;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Helpers;
using SharedLibrary.Models;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Text.RegularExpressions;

namespace MovieTicket.Views.MemberView.BookingView
{
    public class SelectSeatView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly ShowBUS _showBUS;

        public SelectSeatView(IViewFactory viewFactory, ShowBUS showBUS)
        {
            _viewFactory = viewFactory;
            _showBUS = showBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.SelectSeat;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (!SignInManager.IsLogin)
            {
                _viewFactory.GetService(ViewConstant.Start)?.Render();
                return;
            }

            Show? show = _showBUS.GetById((int)(model ?? 0));

            if (show == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("show", ViewConstant.MemberHome);
                return;
            }

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Booking ticket for movie:[/] {show.Movie.Name}");

            List<ShowSeat> showSeats = _showBUS.GetShowSeats(show);

            RenderShowSeats(show.Hall, showSeats);

            AnsiConsole.MarkupLine($"[{ColorConstant.PickedSeat}]'color'[/]: Picked seat");
            AnsiConsole.MarkupLine($"[{ColorConstant.NormalSeat}]'color'[/]: Normal seat");
            AnsiConsole.MarkupLine($"[{ColorConstant.VIPSeat}]'color'[/]: VIP seat\n");

            AnsiConsole.MarkupLine(" * Press [dodgerblue2]'C'[/] to select seats, [red]'ESC'[/] to go back.");

            while (true)
            {
                var key = ConsoleHelper.InputKey(new List<ConsoleKey>()
                    {
                        ConsoleKey.C,
                        ConsoleKey.Escape
                    });

                switch (key)
                {
                    case ConsoleKey.Escape:
                        _viewFactory.GetService(ViewConstant.SelectShow)?.Render(show.Movie.Id);
                        return;
                    case ConsoleKey.C:
                        string seatName = "";
                        while (seatName != "0")
                        {
                            seatName = AnsiConsole.Ask<string>(" -> Enter seat's name separate by ', ' (0 to cancel): ").ToUpper();

                            if (seatName != "0")
                            {
                                string[] seatNameList = Regex.Split(seatName, @", |,");

                                bool valid = true;
                                List<Seat> seatsToAdd = new();

                                // check seat picked or not
                                for (int i = 0; i < seatNameList.Length; i++)
                                {
                                    seatNameList[i] = seatNameList[i].Trim();

                                    if (showSeats.Any(s => s.SeatStatus == SeatStatus.Picked && seatNameList[i] == s.Seat.SeatName))
                                    {
                                        AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Seat '{seatNameList[i]}' is picked !, choose another seat.[/]");
                                        valid = false;
                                        break;
                                    }

                                    if (!showSeats.Any(s => s.Seat.SeatName == seatNameList[i]))
                                    {
                                        AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Seat '{seatNameList[i]}' doesn't exist !, choose another seat.[/]");
                                        valid = false;
                                        break;
                                    }

                                    seatsToAdd.Add(showSeats.Select(s => s.Seat).First(s => s.SeatName == seatNameList[i]));
                                }

                                if (valid)
                                {
                                    _viewFactory.GetService(ViewConstant.ConfirmBooking)?.Render(new BookingModel
                                    {
                                        Show = show,
                                        Seats = seatsToAdd
                                    });
                                    return;
                                }
                            }
                        }

                        break;
                }
            }
        }

        public void RenderShowSeats(Hall hall, List<ShowSeat> showSeats)
        {
            Console.WriteLine();

            Rows rows;
            List<IRenderable> renderables = new();
            
            renderables.Add(new Markup("\n[yellow]SCREEN[/]\n"));
            if (showSeats.Count == 0)
                renderables.Add(new Markup($"[{ColorConstant.Error}]Can not find any seat :((, come back later[/]"));
            
            int pos = 0;
            for (int i = 0; i < hall.Height; i++)
            {
                List<Markup> seatIcons = new();
                for (int j = 0; j < hall.Width; j++)
                {
                    pos++;

                    ShowSeat? seat = showSeats.FirstOrDefault(s => s.Seat.Position == pos);

                    if (seat != null)
                    {
                        // change color base on seat type
                        string color = seat.Seat.SeatType == SeatType.NORMAL ? ColorConstant.NormalSeat : ColorConstant.VIPSeat;
                        color = seat.SeatStatus == SeatStatus.Picked ? ColorConstant.PickedSeat : color;

                        string seatIcon = $"{seat.Seat.SeatName}";

                        if (seatIcon.Length == 2) seatIcon += " ";

                        seatIcons.Add(new Markup($"[{color}]{seatIcon}[/]"));
                    }
                    else
                    {
                        seatIcons.Add(new Markup($"   "));
                    }
                }

                renderables.Add(new Columns(seatIcons));
            }

            rows = new(renderables);

            var panel = new Panel(
                Align.Center(rows))
            {
                Border = BoxBorder.Heavy,
                BorderStyle = new Style(Color.PaleGreen3),
                Expand = true,
                Header = new PanelHeader("Seats")
            };
            panel.Header.Centered();

            AnsiConsole.Write(panel);
        }
    }
}
