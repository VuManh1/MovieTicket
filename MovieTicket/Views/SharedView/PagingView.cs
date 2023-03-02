using SharedLibrary.Constants;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.Shared
{
    public class PagingView : IViewRender
    {
        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            if (model == null) return;

            PagingModel pagingModel = (PagingModel)model;

            // create panel
            var panel = new Panel(
                Align.Center(
                    new Text($"<    [{pagingModel.CurrentPage}/{pagingModel.NumberOfPage}]    >"))
                )
            {
                Border = BoxBorder.Ascii,
                BorderStyle = new Style(Color.PaleGreen3),
                Expand = true
            };

            AnsiConsole.Write(panel);

            AnsiConsole.MarkupLine($" * Press [{ColorConstant.Primary}]'LEFT'[/] or [{ColorConstant.Primary}]'RIGHT'[/] arrow to switch page");
        }
    }
}

