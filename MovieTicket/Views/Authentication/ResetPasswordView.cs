using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.Helpers;
using Spectre.Console;

namespace MovieTicket.Views.Authentication
{
	public class ResetPasswordView : IViewRender
	{
		private readonly AuthenticationBUS _authenticationBus;
		private readonly IViewFactory _viewFactory;

		public ResetPasswordView(AuthenticationBUS authenticationBus, IViewFactory viewFactory)
		{
			_authenticationBus = authenticationBus;
			_viewFactory = viewFactory;
		}

		public void Render(string? statusMessage = null, object? model = null)
		{
			AnsiConsole.MarkupLine($"[{ColorConstant.Title}]Reset Password[/]");

			string newPassword = AnsiConsole.Prompt(
				new TextPrompt<string>(" -> Enter new password: ")
					.PromptStyle("red")
					.Secret());

			// Validate password
			while (!ValidationHelper.CheckPassword(newPassword))
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Password must be between 6 and 30 charactes ![/]");

				newPassword = AnsiConsole.Prompt(
				new TextPrompt<string>(" -> Enter password: ")
					.PromptStyle("red")
					.Secret());
			}

			string confirmPassword = AnsiConsole.Prompt(
				new TextPrompt<string>(" -> Confirm password: ")
					.PromptStyle("red")
					.Secret());

			if (confirmPassword != newPassword)
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Password not match ![/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewFactory.Render("start");
					return;
				}

				_viewFactory.Render("reset_password", model: model);
				return;
			}

			Result result = _authenticationBus.ResetPassword(model?.ToString(), newPassword);
			if (result.Success)
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Reset password successful ![/], press any key to go back.");
				Console.ReadKey();
				_viewFactory.Render("start");
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewFactory.Render("start");
					return;
				}

				_viewFactory.Render("reset_password", model: model);
			}
		}
	}
}
