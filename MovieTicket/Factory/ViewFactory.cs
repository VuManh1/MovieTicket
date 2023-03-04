using Microsoft.Extensions.DependencyInjection;
using MovieTicket.Views;
using MovieTicket.Views.Authentication;
using MovieTicket.Views.Shared;
using SharedLibrary.Constants;

namespace MovieTicket.Factory
{
    public interface IViewFactory
	{
		public IViewRender? GetService(string name);
	}

	public class ViewFactory : IViewFactory
	{
		private readonly IServiceProvider _serviceProvider;

		public ViewFactory(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
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
                ViewConstant.ManageMovie => typeof(Views.AdminView.MovieView.ManageMovieView),
                ViewConstant.AddMovie => typeof(Views.AdminView.MovieView.AddMovieView),
                ViewConstant.AdminListMovie => typeof(Views.AdminView.MovieView.ListMovieView),
                ViewConstant.AdminMovieDetail => typeof(Views.AdminView.MovieView.MovieDetailView),
                ViewConstant.ManageCast => typeof(Views.AdminView.CastView.ManageCastView),
                ViewConstant.AddCast => typeof(Views.AdminView.CastView.AddCastView),
                ViewConstant.AdminListCast => typeof(Views.AdminView.CastView.ListCastView),
                ViewConstant.ManageCinema => typeof(Views.AdminView.CinemaView.ManageCinemaView),
                ViewConstant.AddCinema => typeof(Views.AdminView.CinemaView.AddCinemaView),
                ViewConstant.AdminListCinema => typeof(Views.AdminView.CinemaView.ListCinemaView),
                ViewConstant.ManageDirector => typeof(Views.AdminView.DirectorView.DirectorManageView),
                ViewConstant.AddDirector => typeof(Views.AdminView.DirectorView.AddDirectorView),
                ViewConstant.AdminListDirector => typeof(Views.AdminView.DirectorView.ListDirectorView),
                ViewConstant.ManageHall => typeof(Views.AdminView.HallView.ManageHallView),
                ViewConstant.AddHall => typeof(Views.AdminView.HallView.AddHallView),
                ViewConstant.AdminHallDetail => typeof(Views.AdminView.HallView.HallDetailView),
                ViewConstant.ManageShow => typeof(Views.AdminView.ShowView.ManageShowView),
                ViewConstant.AddShow => typeof(Views.AdminView.ShowView.AddShowView),
                ViewConstant.AdminShowDetail => typeof(Views.AdminView.ShowView.ShowDetailView),
                ViewConstant.AdminListShow => typeof(Views.AdminView.ShowView.ListShowView),
                ViewConstant.AdminListMember => typeof(Views.AdminView.MemberView.ListMemberView),
                ViewConstant.AdminMemberDetail => typeof(Views.AdminView.MemberView.MemberDetailView),
                ViewConstant.AdminCinemaDetail => typeof(Views.AdminView.CinemaView.CinemaDetailView),
                ViewConstant.AdminCastDetail => typeof(Views.AdminView.CastView.CastDetailView),
                ViewConstant.AdminDirectorDetail => typeof(Views.AdminView.DirectorView.DirectorDetailView),
                ViewConstant.MemberHome => typeof(Views.MemberView.HomeView),
                ViewConstant.MovieDetail => typeof(Views.MemberView.MovieView.MovieDetailView),
                ViewConstant.MovieList => typeof(Views.MemberView.MovieView.ListMovieView),
                ViewConstant.SelectShow => typeof(Views.MemberView.BookingView.SelectShowView),
                ViewConstant.SelectSeat => typeof(Views.MemberView.BookingView.SelectSeatView),
                ViewConstant.ConfirmBooking => typeof(Views.MemberView.BookingView.ConfirmBookingView),
                ViewConstant.BookingStatus => typeof(Views.MemberView.BookingView.BookingStatusView),
                ViewConstant.BookingDetail => typeof(Views.MemberView.BookingView.BookingDetailView),
                ViewConstant.ListBooking => typeof(Views.MemberView.BookingView.ListBookingView),
                ViewConstant.MemberDetail => typeof(Views.MemberView.MemberDetailView),
                _ => null
			};

			return _serviceProvider.GetServices<IViewRender>().FirstOrDefault(s => s.GetType() == type);
		}
	}
}
