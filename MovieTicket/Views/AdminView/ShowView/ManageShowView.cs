using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.ShowView
{
    public class ManageShowView : IViewRender
    {
		private readonly IViewFactory _viewFactory;

		public ManageShowView(IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Manage Show]");
        
			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Add Show", "Show All Shows", "Back"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			switch (selection)
			{
				case "Add Show":
                    _viewFactory.Render(ViewConstant.AddShow);
					break;
                case "Show All Shows":
                    _viewFactory.Render(ViewConstant.AdminListShow);
                    break;
				case "Back":
					_viewFactory.Render(ViewConstant.AdminHome);
					break;
			}
        }
    }
}