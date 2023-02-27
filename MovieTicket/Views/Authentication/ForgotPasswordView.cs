using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.Helpers;
using Spectre.Console;

namespace MovieTicket.Views.Authentication
{
	public class ForgotPasswordView : IViewRender
	{
		private readonly AuthenticationBUS _authenticationBus;
		private readonly IViewFactory _viewFactory;

		public ForgotPasswordView(AuthenticationBUS authenticationBus, IViewFactory viewFactory)
		{
			_authenticationBus = authenticationBus;
			_viewFactory = viewFactory;
		}

		public void Render(string? statusMessage = null, object? model = null)
		{
			AnsiConsole.MarkupLine($"[{ColorConstant.Title}]Forgot Password[/]");

			string email = ConsoleHelper.InputEmail(" -> Enter your email: ");

			_authenticationBus.SendOTP(email);

			AnsiConsole.MarkupLine($"[{ColorConstant.Info}]OTP has just been sent to your mailbox.[/]");
			string otp = AnsiConsole.Ask<string>(" -> Enter OTP: ");

			Result result = _authenticationBus.ValidateOTP(otp);
			if (result.Success)
			{
				_viewFactory.Render("reset_password", model: email);
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Invalid OTP ![/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewFactory.Render("start");
					return;
				}

				_viewFactory.Render("forgot_password");
			}
		}
	}
}
