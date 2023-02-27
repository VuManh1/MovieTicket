using BUS;
using DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using MovieTicket.Factory;
using MovieTicket.Views;
using MovieTicket.Views.Authentication;

namespace MovieTicket
{
	public class Application
	{
		public ServiceCollection Services { get; set; }

		public Application()
		{
			Services = new();
		}

		/// <summary>
		/// Add view render services
		/// </summary>
		public void AddViewServices()
		{
			Services.AddScoped<IViewFactory, ViewFactory>();

			Services.AddTransient<IViewRender, StartView>();
			Services.AddTransient<IViewRender, LoginView>();
			Services.AddTransient<IViewRender, RegisterView>();
			Services.AddTransient<IViewRender, ForgotPasswordView>();
			Services.AddTransient<IViewRender, ResetPasswordView>();
			Services.AddTransient<IViewRender, MovieTicket.Views.Admin.HomeView>();
			Services.AddTransient<IViewRender, MovieTicket.Views.Admin.Movie.MovieMenu>();
		}

		/// <summary>
		/// Add Business Services
		/// </summary>
		public void AddBusinesses()
		{
			Services.AddScoped<IUnitOfWork, UnitOfWork>();

			Services.AddTransient<AuthenticationBUS>();
			Services.AddTransient<MovieBus>();
		}

		public void Run()
		{
			var provider = Services.BuildServiceProvider();
			// get StartView and render it
			var view = provider.GetServices<IViewRender>().FirstOrDefault(s => s.GetType() == typeof(StartView));
			
			view?.Render();
		}
	}
}
