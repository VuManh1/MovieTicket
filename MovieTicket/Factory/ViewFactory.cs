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
				ViewConstant.ManageBooking => typeof(Views.AdminView.BookingView.BookingManageView),
                ViewConstant.AddBooking => typeof(Views.AdminView.BookingView.AddBookingView),
                ViewConstant.AdminListBooking => typeof(Views.AdminView.BookingView.ListBookingView),
				ViewConstant.ManageCast => typeof(Views.AdminView.CastView.CastManageView),
                ViewConstant.AddCast => typeof(Views.AdminView.CastView.AddCastView),
                ViewConstant.AdminListCast => typeof(Views.AdminView.CastView.ListCastView),
				ViewConstant.ManageCinema => typeof(Views.AdminView.CinemaView.CinemaManageView),
                ViewConstant.AddCinema => typeof(Views.AdminView.CinemaView.AddCinemaView),
                ViewConstant.AdminListCinema => typeof(Views.AdminView.CinemaView.ListCinemaView),
				ViewConstant.ManageCity => typeof(Views.AdminView.CityView.CityManageView),
                ViewConstant.AddCity => typeof(Views.AdminView.CityView.AddCityView),
                ViewConstant.AdminListCity => typeof(Views.AdminView.CityView.ListCityView),
				ViewConstant.ManageDirector => typeof(Views.AdminView.DirectorView.DirectorManageView),
                ViewConstant.AddDirector => typeof(Views.AdminView.DirectorView.AddDirectorView),
                ViewConstant.AdminListDirector => typeof(Views.AdminView.DirectorView.ListDirectorView),
				ViewConstant.ManageGenre => typeof(Views.AdminView.GenreView.GenreManageView),
                ViewConstant.AddGenre => typeof(Views.AdminView.GenreView.AddGenreView),
                ViewConstant.AdminListGenre => typeof(Views.AdminView.GenreView.ListGenreView),
				ViewConstant.ManageHall => typeof(Views.AdminView.HallView.HallManageView),
                ViewConstant.AddHall => typeof(Views.AdminView.HallView.AddHallView),
                ViewConstant.AdminListHall => typeof(Views.AdminView.HallView.ListHallView),
				ViewConstant.ManageSeat => typeof(Views.AdminView.SeatView.SeatManageView),
                ViewConstant.AddSeat => typeof(Views.AdminView.SeatView.AddSeatView),
                ViewConstant.AdminListSeat => typeof(Views.AdminView.SeatView.ListSeatView),
				ViewConstant.ManageShowSeat => typeof(Views.AdminView.ShowSeatView.ShowSeatManageView),
                ViewConstant.AddShowSeat => typeof(Views.AdminView.ShowSeatView.AddShowSeatView),
                ViewConstant.AdminListShowSeat => typeof(Views.AdminView.ShowSeatView.ListShowSeatView),
				ViewConstant.ManageShow => typeof(Views.AdminView.ShowView.ShowManageView),
                ViewConstant.AddShow => typeof(Views.AdminView.ShowView.AddShowView),
                ViewConstant.AdminListShow => typeof(Views.AdminView.ShowView.ListShowView),
				ViewConstant.ManageUser => typeof(Views.AdminView.UserView.UserManageView),
                ViewConstant.AddUser => typeof(Views.AdminView.UserView.AddUserView),
                ViewConstant.AdminListUser => typeof(Views.AdminView.UserView.ListUserView),
                _ => null
			};

			return _serviceProvider.GetServices<IViewRender>().FirstOrDefault(s => s.GetType() == type);
		}
	}
}
