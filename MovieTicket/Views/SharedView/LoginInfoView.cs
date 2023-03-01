using MovieTicket.SignIn;
using Spectre.Console;

namespace MovieTicket.Views.Shared
{
    public class LoginInfoView : IViewRender
    {
        public void Render(string? statusMessage = null, object? model = null)
        {
            // Render User infor bar:
            if (SignInManager.IsLogin)
            {
                var panel = new Panel(
                    new Markup($"[Gold3_1]{SignInManager.User?.Name} ({SignInManager.User?.Role.ToString()})[/]"))
                {
                    Border = BoxBorder.Heavy,
                    BorderStyle = new Style(Color.PaleGreen3),
                    Expand = true
                };

                AnsiConsole.Write(panel);
            }
        }
    }
}
