using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.HallView
{
    public class HallManageView : IViewRender
    {
		private readonly IViewFactory _viewFactory;

		public HallManageView(IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Manage Hall]");
        
			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Add Hall", "Show All Halls", "Back"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			switch (selection)
			{
				case "Add Hall":
                    _viewFactory.Render(ViewConstant.AddHall);
					break;
                case "Show All Halls":
                    _viewFactory.Render(ViewConstant.AdminListHall);
                    break;
				case "Back":
					_viewFactory.Render(ViewConstant.AdminHome);
					break;
			}
        }
    }
}