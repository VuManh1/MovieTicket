using BUS;
using MovieTicket.Factory;
using MovieTicket.SignIn;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Helpers;
using Spectre.Console;

namespace MovieTicket.Views.Authentication
{
	public class RegisterView : IViewRender
	{
		private readonly AuthenticationBUS _authenticationBUS;
		private readonly CityBUS _cityBUS;
		private readonly IViewFactory _viewFactory;

		public RegisterView(AuthenticationBUS authenticationBUS, IViewFactory viewFactory, CityBUS cityBUS)
		{
			_authenticationBUS = authenticationBUS;
			_viewFactory = viewFactory;
			_cityBUS = cityBUS;
		}

		public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
		{
            Console.Clear();
            Console.Title = ViewConstant.ForgotPassword;

            _viewFactory.GetService(ViewConstant.Logo)?.Render();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Register\n[/]");

			string email = ConsoleHelper.InputEmail();
			string name = ConsoleHelper.InputUserName();
			
			string password = AnsiConsole.Prompt(
				new TextPrompt<string>(" -> Enter password: ")
					.PromptStyle("red")
					.Secret());

			// Validate password
			while (!ValidationHelper.CheckPassword(password))
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Password must be between 6 and 30 charactes ![/]");

				password = AnsiConsole.Prompt(
				new TextPrompt<string>(" -> Enter password: ")
					.PromptStyle("red")
					.Secret());
			}

			// confirm password
			string confirmPassword = AnsiConsole.Prompt(
				new TextPrompt<string>(" -> Confirm password: ")
					.PromptStyle("red")
					.Secret());

			// check password match
			if (confirmPassword != password)
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Password not match ![/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewFactory.GetService(ViewConstant.Start)?.Render();
					return;
				}

				_viewFactory.GetService(ViewConstant.Register)?.Render();
				return;
			}

			// Input phonenumber
			string? phoneNumber = AnsiConsole.Ask<string>(" -> Enter phone number (0 to skip): ");
            // check phonenumber
            while (!ValidationHelper.CheckPhoneNumber(phoneNumber) && phoneNumber != "0")
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Invalid phone number ![/]");
                phoneNumber = AnsiConsole.Ask<string>(" -> Enter phone number (0 to skip): ");
            }

            if (phoneNumber == "0")
			{
				phoneNumber = null;
			}

			// Get city
			string cityName = GetCity();
			City? city = cityName != "Skip" ? _cityBUS.GetByName(cityName) : null;

			User user = new()
			{
				Name = name,
				Email = email,
				PasswordHash = password,
				PhoneNumber = phoneNumber,
				City = city
			};

			// register
			Result result = _authenticationBUS.Register(user);
			if (result.Success)
			{
                SignInManager.SignIn(user);
                _viewFactory.GetService(ViewConstant.MemberHome)?.Render();
            }
            else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewFactory.GetService(ViewConstant.Start)?.Render();
					return;
				}

				_viewFactory.GetService(ViewConstant.Register)?.Render();
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
