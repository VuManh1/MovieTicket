using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Models;
using Spectre.Console;
#pragma warning disable


namespace MovieTicket.Views.AdminView.UserView
{
    public class AddUserView : IViewRender
    {
		private readonly UserBUS _UserBUS;
        private readonly IViewFactory _viewFactory;
        private readonly CityBus _cityBus;


        public AddUserView(UserBUS UserBUS, IViewFactory viewFactory,CityBus cityBus)
		{
			_viewFactory = viewFactory;
            _UserBUS = UserBUS;
            _cityBus = cityBus;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            User User = new();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add User \n[/]");
            
            User.Id = AnsiConsole.Ask<int>(" -> Enter User Id: ");

            User.Name = AnsiConsole.Ask<string>(" -> Enter User Name: ");
            
            User.NormalizeName = AnsiConsole.Ask<string>(" -> Enter Normalize Name: ");

            User.Email = AnsiConsole.Ask<string>(" -> Enter Email: ");
            
            User.PhoneNumber = AnsiConsole.Ask<string>(" -> Enter Phone Number: ");
            
            User.IsLock = AnsiConsole.Ask<bool>(" -> Enter Is Lock: ");
            
            User.PasswordHash = AnsiConsole.Ask<string>(" -> Enter Password Hash: ");
            
            User.Salt = AnsiConsole.Ask<string>(" -> Enter UserCount: ");
            
            User.Role = AnsiConsole.Ask<Role>(" -> Enter Role: ");
            
            User.CreateDate = AnsiConsole.Ask<DateOnly>(" -> Enter Date: ");

            string cityName = GetCity();
			City? city = cityName != "Skip" ? _cityBus.FirstOrDefault($"name = '{cityName}'") : null;


            Result result = _UserBUS.AddBus(User);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add User successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.Render(ViewConstant.ManageUser);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.Render(ViewConstant.ManageUser);
                    return;
                }

                _viewFactory.Render(ViewConstant.AddUser);
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