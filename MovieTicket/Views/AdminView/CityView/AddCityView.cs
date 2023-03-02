using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.CityView
{
    public class AddCityView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly CityBUS _cityBUS;

        public AddCityView(IViewFactory viewFactory, CityBUS cityBUS)
		{
			_viewFactory = viewFactory;
            _cityBUS = cityBUS;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            City city = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add City \n[/]");

            city.Name = AnsiConsole.Ask<string>(" -> Enter City's name: ");

            Result result = _cityBUS.Create(city);
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
			List<string> cities = _cityBUS.GetAll().Select(c => c.Name).ToList();
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