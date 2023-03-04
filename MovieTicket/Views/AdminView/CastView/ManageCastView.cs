using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.CastView
{
    public class ManageCastView : IViewRender
    {
		private readonly IViewFactory _viewFactory;

		public ManageCastView(IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.ManageCast;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Manage Cast]");
        
			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Add Cast", "Show All Casts", "Back"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			switch (selection)
			{
				case "Add Cast":
                    _viewFactory.GetService(ViewConstant.AddCast)?.Render();
					break;
                case "Show All Casts":
                    _viewFactory.GetService(ViewConstant.AdminListCast)?.Render();
                    break;
				case "Back":
					_viewFactory.GetService(ViewConstant.AdminHome)?.Render();
					break;
			}
        }
    }
}