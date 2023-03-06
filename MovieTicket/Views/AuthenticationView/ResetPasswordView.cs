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
		private readonly AuthenticationBUS _authenticationBUS;
		private readonly IViewFactory _viewFactory;

		public ResetPasswordView(AuthenticationBUS authenticationBUS, IViewFactory viewFactory)
		{
			_authenticationBUS = authenticationBUS;
			_viewFactory = viewFactory;
		}

		public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
		{
            Console.Clear();
            Console.Title = ViewConstant.ResetPassword;

            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Reset Password]");

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Reset Password\n[/]");

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
					_viewFactory.GetService(ViewConstant.Start)?.Render();
					return;
				}

				_viewFactory.GetService(ViewConstant.ResetPassword)?.Render(model);
				return;
			}

			Result result = _authenticationBUS.ResetPassword(model?.ToString(), newPassword);
			if (result.Success)
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Reset password successful ![/], press any key to go back.");
				Console.ReadKey();
				_viewFactory.GetService(ViewConstant.Start)?.Render();
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewFactory.GetService(ViewConstant.Start)?.Render();
					return;
				}

				_viewFactory.GetService(ViewConstant.ResetPassword)?.Render(model);
			}
		}
	}
}
