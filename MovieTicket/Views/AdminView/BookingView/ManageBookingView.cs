using MovieTicket.Factory;
using MovieTicket.Views;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.BookingView
{
    public class BookingManageView : IViewRender
    {
		private readonly IViewFactory _viewFactory;

		public BookingManageView(IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Manage Booking]");
        
			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Add Booking", "Show All Bookings", "Back"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			switch (selection)
			{
				case "Add Booking":
                    _viewFactory.Render(ViewConstant.AddBooking);
					break;
                case "Show All Bookings":
                    _viewFactory.Render(ViewConstant.AdminListBooking);
                    break;
				case "Back":
					_viewFactory.Render(ViewConstant.AdminHome);
					break;
			}
        }
    }
}