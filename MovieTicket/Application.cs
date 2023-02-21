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
			Services.AddSingleton<IViewServiceFactory, ViewServiceFactory>();

			Services.AddScoped<IViewRender, StartView>();
			Services.AddScoped<IViewRender, LoginView>();
			Services.AddScoped<IViewRender, RegisterView>();
			Services.AddScoped<IViewRender, ForgotPasswordView>();
			Services.AddScoped<IViewRender, ResetPasswordView>();
		}

		/// <summary>
		/// Add Business Services
		/// </summary>
		public void AddBusinesses()
		{
			Services.AddSingleton<IUnitOfWork, UnitOfWork>();

			Services.AddScoped<AuthenticationBUS>();
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
