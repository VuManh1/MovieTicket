using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.CinemaView
{
    public class AddCinemaView : IViewRender
    {
		private readonly CinemaBUS _cinemaBUS;
        private readonly IViewFactory _viewFactory;
        private readonly CityBUS _cityBus;

        public AddCinemaView(CinemaBUS cinemaBUS, IViewFactory viewFactory, CityBUS cityBus)
		{
			_viewFactory = viewFactory;
            _cinemaBUS = cinemaBUS;
            _cityBus = cityBus;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            Cinema cinema = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Cinema \n[/]");

            cinema.Name = AnsiConsole.Ask<string>(" -> Enter Cinema's name: ");

            cinema.HallCount = AnsiConsole.Ask<int>(" -> Enter Hall Count: ");

            string cityName = GetCity();
			cinema.City = cityName != "Skip" ? _cityBus.FirstOrDefault($"name = '{cityName}'") : null;

            Result result = _cinemaBUS.Create(cinema);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add Cinema successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.Render(ViewConstant.ManageCinema);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.Render(ViewConstant.ManageCinema);
                    return;
                }

                _viewFactory.Render(ViewConstant.AddCinema);
            }
        }

        public string GetCity()
		{
			List<string> cities = _cityBus.GetAll().Select(c => c.Name).ToList();
			cities.Insert(0, "Skip");

			Console.WriteLine();

			// create select city: 
			var city = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("\nChoose a city: ")
					.PageSize(10)
					.AddChoices(cities)
					.HighlightStyle(new Style(Color.PaleGreen3)));

			return city;
		}
    }
}