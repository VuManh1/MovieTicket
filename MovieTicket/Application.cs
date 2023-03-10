using BUS;
using DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MovieTicket.Factory;
using MovieTicket.Views;
using System.Reflection;

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

			// get all class implement IViewRender
			var views = GetAllView();

			foreach (var view in views)
			{
                Services.Add(new ServiceDescriptor(typeof(IViewRender), view, ServiceLifetime.Transient));
            }
		}

		/// <summary>
		/// Add Business Services
		/// </summary>
		public void AddBusinesses()
		{
			Services.AddScoped<IUnitOfWork, UnitOfWork>();

			Services.AddTransient<AuthenticationBUS>();
			Services.AddTransient<MovieBUS>();
			Services.AddTransient<CityBUS>();
			Services.AddTransient<CastBUS>();
            Services.AddTransient<DirectorBUS>();
			Services.AddTransient<BookingBUS>();
            Services.AddTransient<CinemaBUS>();
            Services.AddTransient<GenreBUS>();
            Services.AddTransient<ShowBUS>();
            Services.AddTransient<UserBUS>();
        }

        /// <summary>
        /// Get all type that implement IViewRender
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetAllView()
		{
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(IViewRender).IsAssignableFrom(type) && !type.IsInterface);
        }

		public void Run()
		{
			var provider = Services.BuildServiceProvider();

			// get StartView and render it
			var view = provider.GetServices<IViewRender>()
				.FirstOrDefault(s => s.GetType() == typeof(StartView));

			view?.Render();
		}
	}
}
