using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.DirectorView
{
    public class DirectorManageView : IViewRender
    {
		private readonly IViewFactory _viewFactory;

		public DirectorManageView(IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Manage Director]");
        
			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Add Director", "Show All Directors", "Back"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			switch (selection)
			{
				case "Add Director":
                    _viewFactory.Render(ViewConstant.AddDirector);
					break;
                case "Show All Directors":
                    _viewFactory.Render(ViewConstant.AdminListDirector);
                    break;
				case "Back":
					_viewFactory.Render(ViewConstant.AdminHome);
					break;
			}
        }
    }
}