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
	public class LoginView : IViewRender
	{
		private readonly AuthenticationBUS _authenticationBus;
		private readonly IViewFactory _viewFactory;

		public LoginView(AuthenticationBUS authenticationBus, IViewFactory viewFactory)
		{
			_authenticationBus = authenticationBus;
			_viewFactory = viewFactory;
		}

		public void Render(string? statusMessage = null, object? model = null)
		{
			AnsiConsole.MarkupLine($"[{ColorConstant.Title}]Login[/]");
			AnsiConsole.MarkupLine($"[{ColorConstant.Info}]Type '<forgot>' if you forget your password.[/]");

			// enter email
			string email = AnsiConsole.Ask<string>(" -> Enter email: ");

			if (email == "<forgot>")
			{
				_viewFactory.Render("forgot_password");
				return;
			}

			// check email
			while(!ValidationHelper.CheckEmail(email))
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Email invalid ![/]");
				email = AnsiConsole.Ask<string>(" -> Enter email: ");
			}

			// enter password
			string password = AnsiConsole.Prompt(
				new TextPrompt<string>(" -> Enter password: ")
					.PromptStyle("red")
					.Secret());

			if (password == "<forgot>")
			{
				_viewFactory.Render("forgot_password");
				return;
			}

			// Validate password
			while (!ValidationHelper.CheckPassword(password))
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Password must be between 6 and 30 charactes ![/]");

				password = AnsiConsole.Prompt(
					new TextPrompt<string>(" -> Enter password: ")
						.PromptStyle("red")
						.Secret());
			}

			Result result = _authenticationBus.Login(email, password);
			if (result.Success)
			{
				User? user = (User?)result.Model;

				SignInManager.IsLogin = true;
				SignInManager.User = user;
				
				if(user != null)
				{
					Role userRole = user.Role;

					if(userRole == Role.Admin)
					{
						_viewFactory.Render("AdminHome");
						return;
					}
					else if(userRole == Role.Member)
					{
						return;
					}
				}
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewFactory.Render("start");
					return;
				}

				_viewFactory.Render("login");
			}
		}
	}
}
