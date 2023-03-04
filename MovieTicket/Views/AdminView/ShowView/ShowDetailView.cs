using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary;

namespace MovieTicket.Views.AdminView.ShowView
{
    public class ShowDetailView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly ShowBUS _showBUS;

        public ShowDetailView(IViewFactory viewFactory, ShowBUS showBUS)
        {
            _viewFactory = viewFactory;
            _showBUS = showBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AdminShowDetail;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (model == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("show", ViewConstant.AdminListShow);
                return;
            }

            int showid = (int)model;

            Show? show = _showBUS.GetById(showid);

            if (show == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("show", ViewConstant.AdminListShow);
                return;
            }

            RenderShows(show);

            // check status message
            if (statusMessage != null)
            {
                if (statusMessage.StartsWith("Error"))
                    AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{statusMessage}[/]\n");
                else
                    AnsiConsole.MarkupLine($"[{ColorConstant.Success}]{statusMessage}[/]\n");
            }

            // create select: 
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a action: ")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Go Back", "Delete this show", "Change Start Time"
                    })
                    .HighlightStyle(new Style(Color.PaleGreen3)));


            switch (selection)
            {
                case "Go Back":
                    _viewFactory.GetService(ViewConstant.AdminListShow)?.Render();
                    return;
                case "Delete this show":
                    if (!AnsiConsole.Confirm("Delete this show ? : "))
                    {
                        _viewFactory.GetService(ViewConstant.AdminShowDetail)?.Render(show.Id);
                        return;
                    }

                    Result deleteResult = _showBUS.Delete(show);

                    if (deleteResult.Success)
                        _viewFactory.GetService(ViewConstant.AdminListShow)?.Render();
                    else
                        _viewFactory.GetService(ViewConstant.AdminShowDetail)?.Render(show.Id, statusMessage: "Error !, " + deleteResult.Message);

                    return;
                case "Change Start Time":
                    show.StartTime = AnsiConsole.Ask<DateTime>(" -> Change start time (EX 2-13-2023 14:30:00): ");
                    break;
            }

            Result result = _showBUS.Update(show);

            if (result.Success)
                _viewFactory.GetService(ViewConstant.AdminShowDetail)?.Render(show.Id, statusMessage: "Successful change show detail !");
            else
                _viewFactory.GetService(ViewConstant.AdminShowDetail)?.Render(show.Id, statusMessage: "Error !, " + result.Message);
        }

        public void RenderShows(Show show)
        {
            Rows rows = new(
                new Markup($"[{ColorConstant.Primary}]Id: [/]{show.Id}"),
                new Markup($"[{ColorConstant.Primary}]Movie: [/]{show.Movie.Name}"),
                new Markup($"[{ColorConstant.Primary}]Cinema: [/]{show.Hall.Cinema.Name}"),
                new Markup($"[{ColorConstant.Primary}]Start Time: [/]{show.StartTime.ToString("dd-MM-yyyy HH:mm:ss")}")
            );

            var panel = new Panel(
                Align.Left(rows))
            {
                Border = BoxBorder.Heavy,
                BorderStyle = new Style(Color.PaleGreen3),
                Expand = true,
                Header = new PanelHeader("Show Detail")
            };
            panel.Header.Centered();

            AnsiConsole.Write(panel);
        }
    }
}
