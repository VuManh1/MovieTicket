using MovieTicket.SignIn;
using Spectre.Console;

namespace MovieTicket.Views.Shared
{
    public class LogoView : IViewRender
    {
        public void Render(string? statusMessage = null, object? model = null)
        {
            // create panel
            var panel = new Panel(
                Align.Center(
                    new Rows(
                        new FigletText("MOVIE TICKET")
                        .LeftJustified()
                        .Color(Color.Gold3_1),
                        new Text(statusMessage ?? "")
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

