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
		private readonly IViewFactory _viewFactory;

		public RegisterView(AuthenticationBUS authenticationBus, IViewFactory viewFactory)
		{
			_authenticationBus = authenticationBus;
			_viewFactory = viewFactory;
		}

		public void Render(string? statusMessage = null, object? model = null)
		{
			AnsiConsole.MarkupLine($"[{ColorConstant.Title}]Register[/]");

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
					_viewFactory.Render("start");
					return;
				}

				_viewFactory.Render("register");
				return;
			}

			User user = new()
			{
				Name = name,
				Email = email,
				PasswordHash = password
			};

			// register
			Result result = _authenticationBus.Register(user);
			if (result.Success)
			{
				SignInManager.IsLogin = true;
				SignInManager.User = (User?)result.Model;
				Console.WriteLine(SignInManager.User?.Name);
				Console.WriteLine("Đăng ký thành công !");
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewFactory.Render("start");
					return;
				}

				_viewFactory.Render("register");
			}
		}
	}
}
