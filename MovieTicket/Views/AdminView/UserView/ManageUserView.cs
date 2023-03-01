using MovieTicket.Factory;
using MovieTicket.Views;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.UserView
{
    public class UserManageView : IViewRender
    {
		private readonly IViewFactory _viewFactory;

		public UserManageView(IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();
            _viewFactory.GetService(ViewConstant.Logo)?.Render("[Manage User]");
        
			// create select: 
			var selection = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Add User", "Show All Users", "Back"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			switch (selection)
			{
				case "Add User":
                    _viewFactory.Render(ViewConstant.AddUser);
					break;
                case "Show All Users":
                    _viewFactory.Render(ViewConstant.AdminListUser);
                    break;
				case "Back":
					_viewFactory.Render(ViewConstant.AdminHome);
					break;
			}
        }
    }
}