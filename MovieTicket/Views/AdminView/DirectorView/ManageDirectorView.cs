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
            Console.Clear();
            Console.Title = ViewConstant.ManageDirector;

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
                    _viewFactory.GetService(ViewConstant.AddDirector)?.Render();
					break;
                case "Show All Directors":
                    _viewFactory.GetService(ViewConstant.AdminListDirector)?.Render();
                    break;
				case "Back":
					_viewFactory.GetService(ViewConstant.AdminHome)?.Render();
					break;
			}
        }
    }
}