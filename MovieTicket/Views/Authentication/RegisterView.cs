using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.Authentication
{
	public class RegisterView : IViewRender
	{
		private readonly AuthenticationBUS _authenticationBus;
		private readonly IViewServiceFactory _viewServiceFactory;

		public RegisterView(AuthenticationBUS authenticationBus, IViewServiceFactory viewServiceFactory)
		{
			_authenticationBus = authenticationBus;
			_viewServiceFactory = viewServiceFactory;
		}

		public void Render(string? statusMessage = null, object? model = null)
		{
			Console.Title = "Register";
			AnsiConsole.MarkupLine($"[{ColorConstant.Title}]Register[/]");

			string email = AnsiConsole.Ask<string>(" -> Enter email: ");
			string name = AnsiConsole.Ask<string>(" -> Enter username: ");
			
			string password = AnsiConsole.Prompt(
				new TextPrompt<string>(" -> Enter password: ")
					.PromptStyle("red")
					.Secret());
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
					_viewServiceFactory.Render("start");
					return;
				}

				_viewServiceFactory.Render("register");
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
				Console.WriteLine("Đăng ký thành công !");
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewServiceFactory.Render("start");
					return;
				}

				_viewServiceFactory.Render("register");
			}
		}
	}
}
