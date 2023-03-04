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
        private readonly CityBUS _cityBUS;

        public AddCinemaView(CinemaBUS cinemaBUS, IViewFactory viewFactory, CityBUS cityBUS)
		{
			_viewFactory = viewFactory;
            _cinemaBUS = cinemaBUS;
            _cityBUS = cityBUS;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AddCinema;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            Cinema cinema = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Cinema \n[/]");

            cinema.Name = AnsiConsole.Ask<string>(" -> Enter Cinema's name: ");

            cinema.HallCount = AnsiConsole.Ask<int>(" -> Enter Hall Count: ");

            string cityName = GetCity();
			cinema.City = cityName != "Skip" ? _cityBUS.FirstOrDefault($"name = '{cityName}'") : null;

            Result result = _cinemaBUS.Create(cinema);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add Cinema successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.GetService(ViewConstant.ManageCinema)?.Render();
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.GetService(ViewConstant.ManageCinema)?.Render();
                    return;
                }

                _viewFactory.GetService(ViewConstant.AddCinema)?.Render();
            }
        }

        public string GetCity()
		{
			List<string> cities = _cityBUS.GetAll().Select(c => c.Name).ToList();
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