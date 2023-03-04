using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;

namespace MovieTicket.Views.Shared
{
    public class NotFoundView : IViewRender
    {
        private readonly IViewFactory _viewFactory;

        public NotFoundView(IViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        // Render notfound view when something not found in database
        // statusMessage: name of object not found in database.
        // model: name of view to redirect when press any key.
        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.NotFound;

            AnsiConsole.Markup($"[{ColorConstant.Error}]Can not find any {model?.ToString()} :(([/], press any key to go back.");
            Console.ReadKey();

            _viewFactory.GetService(previousView ?? "")?.Render();
        }
    }
}

