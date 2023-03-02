using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.ShowSeatView
{
    public class ShowSeatManageView : IViewRender
    {
		private readonly IViewFactory _viewFactory;

		public ShowSeatManageView(IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Manage ShowSeat]");
        
			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Add ShowSeat", "Show All ShowSeats", "Back"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			switch (selection)
			{
				case "Add ShowSeat":
                    _viewFactory.Render(ViewConstant.AddShowSeat);
					break;
                case "Show All ShowSeats":
                    _viewFactory.Render(ViewConstant.AdminListShowSeat);
                    break;
				case "Back":
					_viewFactory.Render(ViewConstant.AdminHome);
					break;
			}
        }
    }
}