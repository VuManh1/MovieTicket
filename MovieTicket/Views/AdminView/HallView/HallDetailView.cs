using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace MovieTicket.Views.AdminView.HallView
{
    public class HallDetailView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly CinemaBUS _cinemaBUS;

        public HallDetailView(IViewFactory viewFactory, CinemaBUS cinemaBUS)
        {
            _viewFactory = viewFactory;
            _cinemaBUS = cinemaBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AdminHallDetail;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (model == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("hall", ViewConstant.AdminHome);
                return;
            }

            Hall? hall = (Hall?)model;

            if (hall == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("hall", ViewConstant.AdminHome);
                return;
            }

            // render cinema detail
            RenderHallInfo(hall);

            List<Seat> seats = _cinemaBUS.GetSeats(hall);
            RenderHallSeats(hall, seats);

            // check status message
            if (statusMessage != null)
            {
                if (statusMessage.StartsWith("Error"))
                    AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{statusMessage}[/]\n");
                else
                    AnsiConsole.MarkupLine($"[{ColorConstant.Success}]{statusMessage}[/]\n");
            }

            Console.WriteLine();

            if (seats.Count > 0)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.NormalSeat}]'color'[/]: Normal seat");
                AnsiConsole.MarkupLine($"[{ColorConstant.VIPSeat}]'color'[/]: VIP seat");
            }

            // create select: 
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[PaleGreen3]Choose: [/]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Go Back", "Manage Seat", "Edit hall info"
                    })
                    .HighlightStyle(new Style(Color.PaleGreen3)));

            switch (selection)
            {
                case "Manage Seat":
                    RenderManageSeatView(seats, hall);
                    break;
                case "Edit hall info":
                    RenderEditHallView(hall);
                    break;
                case "Go Back":
                    _viewFactory.GetService(ViewConstant.ManageHall)?.Render(hall.Cinema.Id, ViewConstant.AdminCinemaDetail);
                    break;
            }
        }

        public void RenderManageSeatView(List<Seat> seats, Hall hall)
        {
            // create select: 
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[PaleGreen3]Choose a action: [/]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Go Back", "Add a seat", "Delete a seat"
                    })
                    .HighlightStyle(new Style(Color.PaleGreen3)));

            switch (selection)
            {
                case "Add a seat":
                    Seat seat = new();
                    seat.SeatRow = Char.ToUpper(AnsiConsole.Ask<char>(" -> Enter seat's row (only 1 character 'A', 'B', ...): "));
                    seat.SeatNumber = AnsiConsole.Ask<int>(" -> Enter seat's number: ");

                    while (seats.Any(s => $"{seat.SeatRow}{seat.SeatNumber}" == $"{s.SeatRow}{s.SeatNumber}"))
                    {
                        AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Seat '{seat.SeatName}' already exist ![/]");

                        if (!AnsiConsole.Confirm("Enter again ?: "))
                        {
                            _viewFactory.GetService(ViewConstant.AdminHallDetail)?.Render(hall);
                            return;
                        }

                        seat.SeatRow = Char.ToUpper(AnsiConsole.Ask<char>(" -> Enter seat's row (only 1 character 'A', 'B', ...): "));
                        seat.SeatNumber = AnsiConsole.Ask<int>(" -> Enter seat's number: ");
                    }

                    seat.Position = AnsiConsole.Ask<int>(" -> Enter seat's position (in hall): ");

                    while (seats.Any(s => seat.Position == s.Position))
                    {
                        AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Already have a seat in this position ![/]");

                        if (!AnsiConsole.Confirm("Enter again ?: "))
                        {
                            _viewFactory.GetService(ViewConstant.AdminHallDetail)?.Render(hall);
                            return;
                        }

                        seat.Position = AnsiConsole.Ask<int>(" -> Enter seat's position (in hall): ");
                    }

                    seat.Price = AnsiConsole.Ask<double>(" -> Enter seat's price: ");
                    seat.Hall = hall;
                    seat.SeatType = AnsiConsole.Ask<SeatType>(" -> Enter seat's type ('Normal', 'VIP'): ");

                    Result result = _cinemaBUS.CreateSeat(seat);
                    if (result.Success)
                    {
                        hall.SeatCount++;
                        _viewFactory.GetService(ViewConstant.AdminHallDetail)?.Render(hall, statusMessage: "Add seat successful !");
                    }
                    else
                    {
                        _viewFactory.GetService(ViewConstant.AdminHallDetail)?.Render(hall, statusMessage: $"Error !, add seat failed ! {result.Message}");
                    }

                    return;
                case "Delete a seat":
                    string seatName = AnsiConsole.Ask<string>(" -> Enter seat's name to delete: ").ToUpper();

                    Seat? seatToDelete = seats.FirstOrDefault(s => seatName == $"{s.SeatRow}{s.SeatNumber}");
                    if (seatToDelete == null)
                    {
                        AnsiConsole.Markup($"[{ColorConstant.Error}]Seat '{seatName}' doesn't exist.[/] Enter to continue");
                        Console.ReadKey();
                        _viewFactory.GetService(ViewConstant.AdminHallDetail)?.Render(hall);
                        return;
                    }

                    Result deleteResult = _cinemaBUS.DeleteSeat(seatToDelete);
                    if (deleteResult.Success)
                    {
                        hall.SeatCount--;
                        _viewFactory.GetService(ViewConstant.AdminHallDetail)?.Render(hall, statusMessage: "Delete seat successful !");
                    }
                    else
                    {
                        _viewFactory.GetService(ViewConstant.AdminHallDetail)?.Render(hall, statusMessage: "Error !, delete seat failed !");
                    }

                    return;
                case "Go Back":
                    _viewFactory.GetService(ViewConstant.AdminHallDetail)?.Render(hall);
                    return;
            }
        }

        public void RenderEditHallView(Hall hall)
        {
            // create select: 
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[PaleGreen3]Choose a action: [/]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Go Back", "Change name", "Change width", "Change height"
                    })
                    .HighlightStyle(new Style(Color.PaleGreen3)));

            switch (selection)
            {
                case "Change name":
                    hall.Name = AnsiConsole.Ask<string>(" -> Change hall's name: ");
                    break;
                case "Change width":
                    hall.Width = AnsiConsole.Ask<int>(" -> Change hall's width: ");
                    break;
                case "Change height":
                    hall.Height = AnsiConsole.Ask<int>(" -> Change hall's height: ");
                    break;
                case "Go Back":
                    _viewFactory.GetService(ViewConstant.AdminHallDetail)?.Render(hall);
                    return;
            }

            Result result = _cinemaBUS.UpdateHall(hall);
            if (result.Success)
                _viewFactory.GetService(ViewConstant.AdminHallDetail)?.Render(hall, statusMessage: "Successful change hall detail !");
            else
                _viewFactory.GetService(ViewConstant.AdminHallDetail)?.Render(hall, statusMessage: "Error !, " + result.Message);
        }

        public void RenderHallSeats(Hall hall, List<Seat> seats)
        {
            Console.WriteLine();

            if (seats.Count <= 0)
            {
                AnsiConsole.MarkupLine("No seat");
            }

            Rows rows;
            List<IRenderable> renderables = new();
            renderables.Add(new Markup("\n[yellow]SCREEN[/]\n"));

            int pos = 0;
            for (int i = 0; i < hall.Height; i++)
            {
                List<Markup> seatIcons = new();
                for (int j = 0; j < hall.Width; j++)
                {
                    pos++;

                    Seat? seat = seats.FirstOrDefault(s => s.Position == pos);

                    if (seat != null)
                    {
                        // change color base on seat type
                        string color = seat.SeatType == SeatType.NORMAL ? ColorConstant.NormalSeat : ColorConstant.VIPSeat;
                        string seatIcon = $"{seat.SeatRow}{seat.SeatNumber}";

                        if (seatIcon.Length == 2) seatIcon += " ";

                        seatIcons.Add(new Markup($"[{color}]{seatIcon}[/]"));
                    }
                    else
                    {
                        string seatIcon = pos.ToString();

                        if (seatIcon.Length == 1) seatIcon += "  ";
                        if (seatIcon.Length == 2) seatIcon += " ";

                        seatIcons.Add(new Markup($"{seatIcon}"));
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

        public void RenderHallInfo(Hall hall)
        {
            Rows rows = new(
                new Markup($"[{ColorConstant.Primary}]Id: [/]{hall.Id}"),
                new Markup($"[{ColorConstant.Primary}]Name: [/]{hall.Name}"),
                new Markup($"[{ColorConstant.Primary}]Number of seat: [/]{hall.SeatCount}"),
                new Markup($"[{ColorConstant.Primary}]Cinema: [/]{hall.Cinema.Name}")
            );

            var panel = new Panel(
                Align.Left(rows))
            {
                Border = BoxBorder.Heavy,
                BorderStyle = new Style(Color.PaleGreen3),
                Expand = true,
                Header = new PanelHeader("Hall Detail")
            };
            panel.Header.Centered();

            AnsiConsole.Write(panel);
        }
    }
}
