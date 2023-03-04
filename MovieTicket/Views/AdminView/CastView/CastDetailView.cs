using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.CastView
{
    public class CastDetailView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly CastBUS _castBUS;

        public CastDetailView(CastBUS castBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _castBUS = castBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AdminCastDetail;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (model == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("cast", ViewConstant.AdminListCast);
                return;
            }

            int castid = (int)model;

            Cast? cast = _castBUS.GetById(castid);

            if (cast == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("cast", ViewConstant.AdminListCast);
                return;
            }

            // render movie detail
            RenderCastInfo(cast);

            // check status message
            if (statusMessage != null)
            {
                if (statusMessage.StartsWith("Success"))
                    AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Save changes successful ![/]\n");
                else if (statusMessage.StartsWith("Error"))
                    AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{statusMessage}[/]\n");
            }

                // create select: 
                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose a action: ")
                        .PageSize(10)
                        .AddChoices(new[] {
                        "Go Back", "Delete this cast",
                        "Change Name", "Change 'About'"
                        })
                        .HighlightStyle(new Style(Color.PaleGreen3)));


            switch (selection)
            {
                case "Go Back":
                    _viewFactory.GetService(ViewConstant.AdminListCast)?.Render();
                    return;
                case "Delete this cast":
                    if (!AnsiConsole.Confirm("Delete this cast ? : "))
                    {
                        _viewFactory.GetService(ViewConstant.AdminCastDetail)?.Render(cast.Id);
                        return;
                    }

                    Result deleteResult = _castBUS.Delete(cast);

                    if (deleteResult.Success)
                        _viewFactory.GetService(ViewConstant.AdminListCast)?.Render();
                    else
                        _viewFactory.GetService(ViewConstant.AdminCastDetail)?.Render(cast.Id);
                   
                    return;
                case "Change Name":
                    cast.Name = AnsiConsole.Ask<string>(" -> Change cast's name: ");
                    break;
                case "Change 'About'":
                    cast.About = AnsiConsole.Ask<string>(" -> Change cast's about: ");
                    break;
            }

            Result result = _castBUS.Update(cast);

            if (result.Success)
                _viewFactory.GetService(ViewConstant.AdminCastDetail)?.Render(cast.Id, statusMessage: "Successful change cast detail !");
            else
                _viewFactory.GetService(ViewConstant.AdminCastDetail)?.Render(cast.Id, statusMessage: "Error !, " + result.Message);
        }

        public void RenderCastInfo(Cast cast)
        {
            Rows rows = new(
                new Markup($"[{ColorConstant.Primary}]Id: [/]{cast.Id}"),
                new Markup($"[{ColorConstant.Primary}]Name: [/]{cast.Name}"),
                new Markup($"[{ColorConstant.Primary}]About: [/]{cast.About}")
            );

            var panel = new Panel(
                Align.Left(rows))
            {
                Border = BoxBorder.Heavy,
                BorderStyle = new Style(Color.PaleGreen3),
                Expand = true,
                Header = new PanelHeader("Cast Detail")
            };
            panel.Header.Centered();

            AnsiConsole.Write(panel);
        }
    }
}