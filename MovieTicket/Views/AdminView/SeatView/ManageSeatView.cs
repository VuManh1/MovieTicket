using MovieTicket.Factory;
using MovieTicket.Views;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.SeatView
{
    public class SeatManageView : IViewRender
    {
		private readonly IViewFactory _viewFactory;

		public SeatManageView(IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Manage Seat]");
        
			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Add Seat", "Show All Seats", "Back"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			switch (selection)
			{
				case "Add Seat":
                    _viewFactory.Render(ViewConstant.AddSeat);
					break;
                case "Show All Seats":
                    _viewFactory.Render(ViewConstant.AdminListSeat);
                    break;
				case "Back":
					_viewFactory.Render(ViewConstant.AdminHome);
					break;
			}
        }
    }
}