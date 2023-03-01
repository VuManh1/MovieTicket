using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.CinemaView
{
    public class AddCinemaView : IViewRender
    {
		private readonly CinemaBus _CinemaBUS;
        private readonly IViewFactory _viewFactory;
        private readonly CityBus _cityBus;

        public AddCinemaView(CinemaBus CinemaBUS, IViewFactory viewFactory,CityBus cityBus)
		{
			_viewFactory = viewFactory;
            _CinemaBUS = CinemaBUS;
            _cityBus = cityBus;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            Cinema Cinema = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Cinema \n[/]");

            Cinema.Name = AnsiConsole.Ask<string>(" -> Enter Cinema's name: ");

            Cinema.HallCount = AnsiConsole.Ask<int>(" -> Enter Hall Count: ");

            string cityName = GetCity();
			City? city = cityName != "Skip" ? _cityBus.FirstOrDefault($"name = '{cityName}'") : null;

            Result result = _CinemaBUS.AddBus(Cinema);
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
					.Title("\nChoose a city where you live: ")
					.PageSize(10)
					.AddChoices(cities)
					.HighlightStyle(new Style(Color.PaleGreen3)));

			return city;
		}
    }
}