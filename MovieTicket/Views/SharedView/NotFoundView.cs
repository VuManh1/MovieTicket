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
        public void Render(string? statusMessage = null, object? model = null)
        {
            AnsiConsole.Markup($"[{ColorConstant.Error}]Can not find any {statusMessage} :(([/], press any key to go back.");
            Console.ReadKey();

            _viewFactory.Render(model?.ToString() ?? "");
        }
    }
}

