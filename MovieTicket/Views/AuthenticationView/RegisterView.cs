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
		private readonly AuthenticationBUS _authenticationBus;
		private readonly CityBUS _cityBus;
		private readonly IViewFactory _viewFactory;

		public RegisterView(AuthenticationBUS authenticationBus, IViewFactory viewFactory, CityBUS cityBus)
		{
			_authenticationBus = authenticationBus;
			_viewFactory = viewFactory;
			_cityBus = cityBus;
		}

		public void Render(string? statusMessage = null, object? model = null)
		{
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
					_viewFactory.Render(ViewConstant.Start);
					return;
				}

				_viewFactory.Render(ViewConstant.Register);
				return;
			}

			// Input phonenumber
			string? phoneNumber = AnsiConsole.Ask<string>(" -> Enter phone number (0 to skip): ");
			if(phoneNumber == "0")
			{
				phoneNumber = null;
			}
			else
			{
				// check email
				while (!ValidationHelper.CheckPhoneNumber(phoneNumber))
				{
					AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Invalid phone number ![/]");
					phoneNumber = AnsiConsole.Ask<string>(" -> Enter phone number (0 to skip): ");
				}
			}

			// Get city
			string cityName = GetCity();
			City? city = cityName != "Skip" ? _cityBus.FirstOrDefault($"name = '{cityName}'") : null;

			User user = new()
			{
				Name = name,
				Email = email,
				PasswordHash = password,
				PhoneNumber = phoneNumber,
				City = city
			};

			// register
			Result result = _authenticationBus.Register(user);
			if (result.Success)
			{
				SignInManager.SignIn(user);
				Console.WriteLine("Đăng ký thành công !");
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewFactory.Render(ViewConstant.Start);
					return;
				}

				_viewFactory.Render(ViewConstant.Register);
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
