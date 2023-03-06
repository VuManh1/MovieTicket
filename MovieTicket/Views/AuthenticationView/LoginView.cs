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
		private readonly AuthenticationBUS _authenticationBUS;
		private readonly IViewFactory _viewFactory;

		public LoginView(AuthenticationBUS authenticationBUS, IViewFactory viewFactory)
		{
			_authenticationBUS = authenticationBUS;
			_viewFactory = viewFactory;
		}

		public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
		{
			Console.Clear();
			Console.Title = ViewConstant.Login;

            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Login]");

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Login\n[/]");
			AnsiConsole.MarkupLine($"[{ColorConstant.Info}]Type '<forgot>' if you forget your password.[/]");

			// enter email
			string email = AnsiConsole.Ask<string>(" -> Enter email: ");

			if (email == "<forgot>")
			{
				_viewFactory.GetService(ViewConstant.ForgotPassword)?.Render();
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
				_viewFactory.GetService(ViewConstant.ForgotPassword)?.Render();
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

			Result result = _authenticationBUS.Login(email, password);
			if (result.Success)
			{
				User? user = (User?)result.Model;

				if (user == null) return;

				SignInManager.SignIn(user);

				if (user.Role == Role.Admin)
				{
					_viewFactory.GetService(ViewConstant.AdminHome)?.Render();
					return;
				}
				else if (user.Role == Role.Member)
				{
					_viewFactory.GetService(ViewConstant.MemberHome)?.Render();
					return;
				}
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewFactory.GetService(ViewConstant.Start)?.Render();
					return;
				}

				_viewFactory.GetService(ViewConstant.Login)?.Render();
			}
		}
	}
}
