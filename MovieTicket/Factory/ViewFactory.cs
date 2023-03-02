using Microsoft.Extensions.DependencyInjection;
using MovieTicket.Views;
using MovieTicket.Views.Authentication;
using MovieTicket.Views.Shared;
using SharedLibrary.Constants;

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
                ViewConstant.NotFound => typeof(NotFoundView),
                ViewConstant.Paging => typeof(PagingView),
                ViewConstant.Logo => typeof(LogoView),
                ViewConstant.LoginInfo => typeof(LoginInfoView),
                ViewConstant.Start => typeof(StartView),
                ViewConstant.Login => typeof(LoginView),
                ViewConstant.Register => typeof(RegisterView),
                ViewConstant.ForgotPassword => typeof(ForgotPasswordView),
                ViewConstant.ResetPassword => typeof(ResetPasswordView),
                ViewConstant.AdminHome => typeof(Views.AdminView.HomeView),
                ViewConstant.ManageMovie => typeof(Views.AdminView.MovieView.MovieManageView),
                ViewConstant.AddMovie => typeof(Views.AdminView.MovieView.AddMovieView),
                ViewConstant.AdminListMovie => typeof(Views.AdminView.MovieView.ListMovieView),
                ViewConstant.AdminMovieDetail => typeof(Views.AdminView.MovieView.MovieDetailView),
                _ => null
			};

			return _serviceProvider.GetServices<IViewRender>().FirstOrDefault(s => s.GetType() == type);
		}
	}
}
