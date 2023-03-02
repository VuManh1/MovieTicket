using MovieTicket.SignIn;
using Spectre.Console;

namespace MovieTicket.Views.Shared
{
    public class LogoView : IViewRender
    {
        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            // create panel
            var panel = new Panel(
                Align.Center(
                    new Rows(
                        new FigletText("MOVIE TICKET")
                        .LeftJustified()
                        .Color(Color.Gold3_1),
                        new Text(model?.ToString() ?? "")
                    )
                ))
            {
                Border = BoxBorder.Heavy,
                BorderStyle = new Style(Color.PaleGreen3),
                Padding = new Padding(2, 2, 2, 2),
                Expand = true
            };

            AnsiConsole.Write(panel);
        }
    }
}

