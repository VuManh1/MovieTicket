using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.CityView
{
    public class AddCityView : IViewRender
    {
		private readonly CityBUS _CityBUS;
        private readonly IViewFactory _viewFactory;
        private readonly CityBUS _cityBus;

        public AddCityView(CityBUS CityBUS, IViewFactory viewFactory,CityBUS cityBus)
		{
			_viewFactory = viewFactory;
            _CityBUS = CityBUS;
            _cityBus = cityBus;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            City City = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add City \n[/]");

            City.Name = AnsiConsole.Ask<string>(" -> Enter City's name: ");

            Result result = _CityBUS.Create(City);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add City successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.Render(ViewConstant.ManageCity);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.Render(ViewConstant.ManageCity);
                    return;
                }

                _viewFactory.Render(ViewConstant.AddCity);
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