using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.DirectorView
{
    public class DirectorDetailView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly DirectorBUS _directorBUS;

        public DirectorDetailView(DirectorBUS directorBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _directorBUS = directorBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AdminDirectorDetail;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (model == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("director", ViewConstant.AdminListDirector);
                return;
            }

            int directorid = (int)model;

            Director? director = _directorBUS.GetById(directorid);

            if (director == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("director", ViewConstant.AdminListDirector);
                return;
            }

            // render movie detail
            RenderDirectorInfo(director);

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
                        .Title("Choose a actiont: ")
                        .PageSize(10)
                        .AddChoices(new[] {
                        "Go Back", "Delete this director", 
                        "Change Name", "Change 'About'"
                        })
                        .HighlightStyle(new Style(Color.PaleGreen3)));


            switch (selection)
            {
                case "Go Back":
                    _viewFactory.GetService(ViewConstant.AdminListDirector)?.Render();
                    return;
                case "Delete this director":
                    if (!AnsiConsole.Confirm("Delete this director ? : "))
                    {
                        _viewFactory.GetService(ViewConstant.AdminDirectorDetail)?.Render(director.Id);
                        return;
                    }

                    Result deleteResult = _directorBUS.Delete(director);

                    if (deleteResult.Success)
                        _viewFactory.GetService(ViewConstant.AdminListDirector)?.Render();
                    else
                        _viewFactory.GetService(ViewConstant.AdminDirectorDetail)?.Render(director.Id, statusMessage: "Error !, " + deleteResult.Message);

                    return;
                case "Change Name":
                    director.Name = AnsiConsole.Ask<string>(" -> Change director's name: ");
                    break;
                case "Change 'About'":
                    director.About = AnsiConsole.Ask<string>(" -> Change director's about: ");
                    break;
            }

            Result result = _directorBUS.Update(director);

            if (result.Success)
                _viewFactory.GetService(ViewConstant.AdminDirectorDetail)?.Render(director.Id, statusMessage: "Successful change director detail !");
            else
                _viewFactory.GetService(ViewConstant.AdminDirectorDetail)?.Render(director.Id, statusMessage: "Error !, " + result.Message);
        }

        public void RenderDirectorInfo(Director director)
        {
            Rows rows = new(
                new Markup($"[{ColorConstant.Primary}]Id: [/]{director.Id}"),
                new Markup($"[{ColorConstant.Primary}]Name: [/]{director.Name}"),
                new Markup($"[{ColorConstant.Primary}]About: [/]{director.About}")
            );

            var panel = new Panel(
                Align.Left(rows))
            {
                Border = BoxBorder.Heavy,
                BorderStyle = new Style(Color.PaleGreen3),
                Expand = true,
                Header = new PanelHeader("Director Detail")
            };
            panel.Header.Centered();

            AnsiConsole.Write(panel);
        }
    }
}