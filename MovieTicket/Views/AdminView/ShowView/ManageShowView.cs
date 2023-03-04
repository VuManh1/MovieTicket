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
			Console.Clear();
			Console.Title = ViewConstant.ManageShow;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Manage Show]");
        
			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Add a Show", "Edit Shows", "Back"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			switch (selection)
			{
				case "Add a Show":
                    _viewFactory.GetService(ViewConstant.AddShow)?.Render();
					break;
                case "Edit Shows":
                    _viewFactory.GetService(ViewConstant.AdminListShow)?.Render();
                    break;
				case "Back":
					_viewFactory.GetService(ViewConstant.AdminHome)?.Render();
					break;
			}
        }
    }
}