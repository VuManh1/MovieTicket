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

        public void Render(string? statusMessage = null, object? model = null)
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
                        .Title("Choose a field you want to edit: ")
                        .PageSize(10)
                        .AddChoices(new[] {
                        "Go Back", "Delete this cinema", 
                        "Name", "Number Of Halls", "Address", "City", 
                        })
                        .HighlightStyle(new Style(Color.PaleGreen3)));


            switch (selection)
            {
                case "Go Back":
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
                        _viewFactory.Render(ViewConstant.AdminCinemaDetail, "Error !, " + deleteResult.Message, cinema.Id);

                    return;
                case "Name":
                    cinema.Name = AnsiConsole.Ask<string>(" -> Change cinema's name: ");
                    break;
                case "Number Of Halls":
                    cinema.HallCount = AnsiConsole.Ask<int>(" -> Change number of halls: ");
                    break;
                case "Address":
                    cinema.Address = AnsiConsole.Ask<string>(" -> Change cinema's address: ");
                    break;
                case "City":
                    cinema.City = GetCity();
                    break;
            }

            Result result = _cinemaBUS.Update(cinema);

            if (result.Success)
                _viewFactory.Render(ViewConstant.AdminCinemaDetail, "Successful change cinema detail !", cinema.Id);
            else
                _viewFactory.Render(ViewConstant.AdminCinemaDetail, "Error !, " + result.Message, cinema.Id);
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
            Rows rows = new(
                new Markup($"[{ColorConstant.Primary}]Id: [/]{cinema.Id}"),
                new Markup($"[{ColorConstant.Primary}]Name: [/]{cinema.Name}"),
                new Markup($"[{ColorConstant.Primary}]Number Of Halls: [/]{cinema.HallCount}"),
                new Markup($"[{ColorConstant.Primary}]Address: [/]{cinema.Address}"),
                new Markup($"[{ColorConstant.Primary}]City: [/]{cinema.City?.Name}")
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