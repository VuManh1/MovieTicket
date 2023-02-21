using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.Authentication
{
	public class LoginView : IViewRender
	{
		private readonly AuthenticationBUS _authenticationBus;
		private readonly IViewServiceFactory _viewServiceFactory;

		public LoginView(AuthenticationBUS authenticationBus, IViewServiceFactory viewServiceFactory)
		{
			_authenticationBus = authenticationBus;
			_viewServiceFactory = viewServiceFactory;
		}

		public void Render(string? statusMessage = null, object? model = null)
		{
			Console.Title = "Login";
			AnsiConsole.MarkupLine($"[{ColorConstant.Title}]Login[/]");
			AnsiConsole.MarkupLine($"[{ColorConstant.Info}]Type '<forgot>' if you forget your password.[/]");

			// enter email
			string email = AnsiConsole.Ask<string>(" -> Enter email: ");
			
			if (email == "<forgot>")
			{
				_viewServiceFactory.Render("forgot_password");
				return;
			}
				
			// enter password
			string password = AnsiConsole.Prompt(
				new TextPrompt<string>(" -> Enter password: ")
					.PromptStyle("red")
					.Secret());

			if (password == "<forgot>")
			{
				_viewServiceFactory.Render("forgot_password");
				return;
			}

			Result result = _authenticationBus.Login(email, password);
			if (result.Success)
			{
				Console.WriteLine("Đăng nhập thành công !");
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewServiceFactory.Render("start");
					return;
				}

				_viewServiceFactory.Render("login");
			}
		}
	}
}
