using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.Authentication
{
	public class ResetPasswordView : IViewRender
	{
		private readonly AuthenticationBUS _authenticationBus;
		private readonly IViewServiceFactory _viewServiceFactory;

		public ResetPasswordView(AuthenticationBUS authenticationBus, IViewServiceFactory viewServiceFactory)
		{
			_authenticationBus = authenticationBus;
			_viewServiceFactory = viewServiceFactory;
		}

		public void Render(string? statusMessage = null, object? model = null)
		{
			AnsiConsole.MarkupLine($"[{ColorConstant.Title}]Reset Password[/]");

			string newPassword = AnsiConsole.Prompt(
				new TextPrompt<string>(" -> Enter new password: ")
					.PromptStyle("red")
					.Secret());

			string confirmPassword = AnsiConsole.Prompt(
				new TextPrompt<string>(" -> Confirm password: ")
					.PromptStyle("red")
					.Secret());

			if (confirmPassword != newPassword)
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Password not match ![/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewServiceFactory.Render("start");
					return;
				}

				_viewServiceFactory.Render("reset_password", model: model);
				return;
			}

			Result result = _authenticationBus.ResetPassword(model?.ToString(), newPassword);
			if (result.Success)
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Reset password successful ![/], press any key to go back.");
				Console.ReadKey();
				_viewServiceFactory.Render("start");
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewServiceFactory.Render("start");
					return;
				}

				_viewServiceFactory.Render("reset_password", model: model);
			}
		}
	}
}
