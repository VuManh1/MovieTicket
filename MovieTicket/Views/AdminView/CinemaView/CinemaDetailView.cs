using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.CinemaView
{
    public class CinemaDetailView : IViewRender
    {
		private readonly CinemaBUS _cinemaBUS;
        private readonly IViewFactory _viewFactory;
        private readonly CityBUS _cityBus;

        public CinemaDetailView(CinemaBUS cinemaBUS, CityBUS cityBus, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _cinemaBUS = cinemaBUS;
            _cityBus = cityBus;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (model == null)
            {
                _viewFactory.Render(ViewConstant.NotFound, "cinema", ViewConstant.AdminListCinema);
                return;
            }

            int cinemaid = (int)model;

            Cinema? cinema = _cinemaBUS.GetById(cinemaid);

            if (cinema == null)
            {
                _viewFactory.Render(ViewConstant.NotFound, "cinema", ViewConstant.AdminListCinema);
                return;
            }

            // render cinema detail
            RenderCinemaInfo(cinema);

            // check status message
            if (statusMessage != null)
            {
                if (statusMessage.StartsWith("Error"))
                    AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{statusMessage}[/]\n");
                else
                    AnsiConsole.MarkupLine($"[{ColorConstant.Success}]{statusMessage}[/]\n");
            }

            // create select: 
            var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose a action: ")
                        .PageSize(10)
                        .AddChoices(new[] {
                        "Go Back", "Delete this cinema", "Add a hall", "Delete a hall",
                        "Change Name", "Change Number Of Halls", "Change Address", "Change City"
                        })
                        .HighlightStyle(new Style(Color.PaleGreen3)));


            switch (selection)
            {
                case "Go Back":
                    _viewFactory.Render(ViewConstant.AdminListCinema);
                    return;
                case "Add a hall":
                    _viewFactory.Render(ViewConstant.AdminListCinema);
                    return;
                case "Delete a hall":
                    _viewFactory.Render(ViewConstant.AdminListCinema);
                    return;
                case "Delete this cinema":
                    if (!AnsiConsole.Confirm("Delete this cinema ? : "))
                    {
                        _viewFactory.Render(ViewConstant.AdminCinemaDetail, model:cinema.Id);
                        return;
                    }

                    Result deleteResult = _cinemaBUS.Delete(cinema);

                    if (deleteResult.Success)
                        _viewFactory.Render(ViewConstant.AdminListCinema);
                    else
                        _viewFactory.Render(ViewConstant.AdminCinemaDetail, cinema.Id, statusMessage: "Error !, " + deleteResult.Message);

                    return;
                case "Change Name":
                    cinema.Name = AnsiConsole.Ask<string>(" -> Change cinema's name: ");
                    break;
                case "Change Number Of Halls":
                    cinema.HallCount = AnsiConsole.Ask<int>(" -> Change number of halls: ");
                    break;
                case "Change Address":
                    cinema.Address = AnsiConsole.Ask<string>(" -> Change cinema's address: ");
                    break;
                case "Change City":
                    cinema.City = GetCity();
                    break;
            }

            Result result = _cinemaBUS.Update(cinema);

            if (result.Success)
                _viewFactory.Render(ViewConstant.AdminCinemaDetail, cinema.Id, statusMessage: "Successful change cinema detail !");
            else
                _viewFactory.Render(ViewConstant.AdminCinemaDetail, cinema.Id, statusMessage: "Error !, " + result.Message);
        }

        public City? GetCity()
        {
            List<string> cities = _cityBus.GetAll().Select(c => c.Name).ToList();
            cities.Insert(0, "Skip");

            Console.WriteLine();

            // create select city: 
            var city = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\nChange city: ")
                    .PageSize(10)
                    .AddChoices(cities)
                    .HighlightStyle(new Style(Color.PaleGreen3)));

            return city != "Skip" ? _cityBus.FirstOrDefault($"name = '{city}'") : null;
        }

        public void RenderCinemaInfo(Cinema cinema)
        {
            List<Hall> halls = _cinemaBUS.GetHalls(cinema);

            Rows rows = new(
                new Markup($"[{ColorConstant.Primary}]Id: [/]{cinema.Id}"),
                new Markup($"[{ColorConstant.Primary}]Name: [/]{cinema.Name}"),
                new Markup($"[{ColorConstant.Primary}]Number Of Halls: [/]{cinema.HallCount}"),
                new Markup($"[{ColorConstant.Primary}]Address: [/]{cinema.Address}"),
                new Markup($"[{ColorConstant.Primary}]City: [/]{cinema.City?.Name}\n"),
                new Markup($"[{ColorConstant.Primary}]Halls: [/]{String.Join(", ", halls.Select(h => h.Name))}")
            );

            var panel = new Panel(
                Align.Left(rows))
            {
                Border = BoxBorder.Heavy,
                BorderStyle = new Style(Color.PaleGreen3),
                Expand = true,
                Header = new PanelHeader("Cinema Detail")
            };
            panel.Header.Centered();

            AnsiConsole.Write(panel);
        }
    }
}