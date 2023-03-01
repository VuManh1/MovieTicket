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
            _viewFactory.GetService(ViewConstant.Logo)?.Render();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Login\n[/]");
			AnsiConsole.MarkupLine($"[{ColorConstant.Info}]Type '<forgot>' if you forget your password.[/]");

			// enter email
			string email = AnsiConsole.Ask<string>(" -> Enter email: ");

			if (email == "<forgot>")
			{
				_viewFactory.Render(ViewConstant.ForgotPassword);
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
				_viewFactory.Render(ViewConstant.ForgotPassword);
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

				if (user == null) return;

				SignInManager.SignIn(user);

				if (user.Role == Role.Admin)
				{
					_viewFactory.Render(ViewConstant.AdminHome);
					return;
				}
				else if (user.Role == Role.Member)
				{
					return;
				}
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewFactory.Render(ViewConstant.Start);
					return;
				}

				_viewFactory.Render(ViewConstant.Login);
			}
		}
	}
}
