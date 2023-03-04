using MovieTicket.SignIn;
using Spectre.Console;

namespace MovieTicket.Views.Shared
{
    public class LoginInfoView : IViewRender
    {
        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            // Render User infor bar:
            if (SignInManager.IsLogin)
            {
                var panel = new Panel(
                    new Markup($"[Gold3_1]{SignInManager.User?.Name} ({SignInManager.User?.Role.ToString()})[/]"))
                {
                    Border = BoxBorder.Ascii,
                    BorderStyle = new Style(Color.PaleGreen3),
                    Expand = true
                };

                AnsiConsole.Write(panel);
                Console.WriteLine();
            }
        }
    }
}
