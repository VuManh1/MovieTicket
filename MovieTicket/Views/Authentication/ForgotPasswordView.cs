using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.Authentication
{
	public class ForgotPasswordView : IViewRender
	{
		private readonly AuthenticationBUS _authenticationBus;
		private readonly IViewServiceFactory _viewServiceFactory;

		public ForgotPasswordView(AuthenticationBUS authenticationBus, IViewServiceFactory viewServiceFactory)
		{
			_authenticationBus = authenticationBus;
			_viewServiceFactory = viewServiceFactory;
		}

		public void Render(string? statusMessage = null, object? model = null)
		{
			AnsiConsole.MarkupLine($"[{ColorConstant.Title}]Forgot Password[/]");

			string email = AnsiConsole.Ask<string>(" -> Enter your email: ");

			_authenticationBus.SendOTP(email);

			AnsiConsole.MarkupLine($"[{ColorConstant.Info}]OTP has just been sent to your mailbox.[/]");
			string otp = AnsiConsole.Ask<string>(" -> Enter OTP: ");

			Result result = _authenticationBus.ValidateOTP(otp);
			if (result.Success)
			{
				_viewServiceFactory.Render("reset_password", model: email);
			}
			else
			{
				AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Invalid OTP ![/]");

				if (!AnsiConsole.Confirm("Continue ? : "))
				{
					_viewServiceFactory.Render("start");
					return;
				}

				_viewServiceFactory.Render("forgot_password");
			}
		}
	}
}
