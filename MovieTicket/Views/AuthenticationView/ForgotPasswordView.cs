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

		public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
		{
            _viewFactory.GetService(ViewConstant.Logo)?.Render();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Forgot Password\n[/]");

			string email = ConsoleHelper.InputEmail(" -> Enter your email: ");

			// send OTP
			_authenticationBus.SendOTP(email);

			AnsiConsole.MarkupLine($"[{ColorConstant.Info}]OTP has just been sent to your mailbox.[/]");
			AnsiConsole.MarkupLine($"[{ColorConstant.Info}]OTP code will expire in 60 seconds.[/]");
            string otp = AnsiConsole.Ask<string>(" -> Enter OTP: ");

			Result result = _authenticationBus.ValidateOTP(otp);
			if (result.Success)
			{
				_viewFactory.Render(ViewConstant.ResetPassword, model: email);
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Invalid OTP ![/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewFactory.Render(ViewConstant.Start);
					return;
				}

				_viewFactory.Render(ViewConstant.ForgotPassword);
			}
		}
	}
}
