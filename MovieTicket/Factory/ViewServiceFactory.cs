using Microsoft.Extensions.DependencyInjection;
using MovieTicket.Views;
using MovieTicket.Views.Authentication;

namespace MovieTicket.Factory
{
	public interface IViewServiceFactory
	{
		public void Render(string name, string? statusMessage = null, object? model = null);
		public IViewRender? GetService(string name);
	}

	public class ViewServiceFactory : IViewServiceFactory
	{
		private readonly IServiceProvider _serviceProvider;

		public ViewServiceFactory(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public void Render(string name, string? statusMessage = null, object? model = null)
		{
			Console.Clear();

			// get view by name :
			IViewRender? view = this.GetService(name);
			view?.Render(statusMessage, model);
		}

		public IViewRender? GetService(string name)
		{
			Type? type = name switch
			{
				"start" => typeof(StartView),
				"login" => typeof(LoginView),
				"register" => typeof(RegisterView),
				"forgot_password" => typeof(ForgotPasswordView),
				"reset_password" => typeof(ResetPasswordView),
				_ => null
			};

			return _serviceProvider.GetServices<IViewRender>().FirstOrDefault(s => s.GetType() == type);
		}
	}
}
