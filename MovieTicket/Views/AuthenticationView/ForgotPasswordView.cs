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
		private readonly AuthenticationBUS _authenticationBUS;
		private readonly IViewFactory _viewFactory;

		public ForgotPasswordView(AuthenticationBUS authenticationBUS, IViewFactory viewFactory)
		{
			_authenticationBUS = authenticationBUS;
			_viewFactory = viewFactory;
		}

		public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
		{
			Console.Clear();
			Console.Title = ViewConstant.ForgotPassword;

            _viewFactory.GetService(ViewConstant.Logo)?.Render();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Forgot Password\n[/]");

			string email = ConsoleHelper.InputEmail(" -> Enter your email: ");

			// send OTP
			_authenticationBUS.SendOTP(email);

			AnsiConsole.MarkupLine($"[{ColorConstant.Info}]OTP has just been sent to your mailbox.[/]");
			AnsiConsole.MarkupLine($"[{ColorConstant.Info}]OTP code will expire in 60 seconds.[/]");
            string otp = AnsiConsole.Ask<string>(" -> Enter OTP: ");

			Result result = _authenticationBUS.ValidateOTP(otp);
			if (result.Success)
			{
				_viewFactory.GetService(ViewConstant.ResetPassword)?.Render(email);
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Invalid OTP ![/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewFactory.GetService(ViewConstant.Start)?.Render();
					return;
				}

				_viewFactory.GetService(ViewConstant.ForgotPassword)?.Render();
			}
		}
	}
}
