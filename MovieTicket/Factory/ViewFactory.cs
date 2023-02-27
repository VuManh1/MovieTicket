using Microsoft.Extensions.DependencyInjection;
using MovieTicket.Views;
using MovieTicket.Views.Authentication;

namespace MovieTicket.Factory
{
	public interface IViewFactory
	{
		public void Render(string name, string? statusMessage = null, object? model = null);
		public IViewRender? GetService(string name);
	}

	public class ViewFactory : IViewFactory
	{
		private readonly IServiceProvider _serviceProvider;

		public ViewFactory(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public void Render(string name, string? statusMessage = null, object? model = null)
		{
			Console.Clear();
			Console.Title = name;

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
				"AdminHome" => typeof(MovieTicket.Views.Admin.HomeView),
				"AdminMovieMenu" => typeof(MovieTicket.Views.Admin.Movie.MovieMenu),
				_ => null
			};

			return _serviceProvider.GetServices<IViewRender>().FirstOrDefault(s => s.GetType() == type);
		}
	}
}
