using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Helpers;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.HallView
{
    public class ManageHallView : IViewRender
    {
		private readonly IViewFactory _viewFactory;
		private readonly CinemaBUS _cinemaBUS;

        public ManageHallView(IViewFactory viewFactory, CinemaBUS cinemaBUS)
		{
			_viewFactory = viewFactory;
			_cinemaBUS = cinemaBUS;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.ManageHall;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

			int cinemaid;
			if (model == null)
			{
				cinemaid = AnsiConsole.Ask<int>(" -> Enter cinema id to manage it's hall (or 0 to go back): ");

				if (cinemaid == 0)
				{
					_viewFactory.GetService(previousView ?? ViewConstant.AdminHome)?.Render();
					return;
				}
			}
			else
			{
				cinemaid = (int)model;
			}

			Cinema? cinema = _cinemaBUS.GetById(cinemaid);

			if (cinema == null)
			{
				_viewFactory.GetService(ViewConstant.NotFound)?.Render("cinema", previousView ?? ViewConstant.AdminHome);
				return;
            }

			// get halls:
			List<Hall> halls = _cinemaBUS.GetHalls(cinema);

            // create panel
            var titlePanel = new Panel(
				Align.Center(
					new Rows(
						new Markup($"[{ColorConstant.Primary}]Manage hall for cinema:[/] {cinema.Name}\n"),
                        new Text($"List of hall: {String.Join(", ", halls.Select(h => h.Name))}.")
					)
				))
			{
                Border = BoxBorder.Ascii,
                BorderStyle = new Style(Color.PaleGreen3),
                Expand = true
            };
            AnsiConsole.Write(titlePanel);

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
					.Title("[PaleGreen3]Choose: [/]")
					.PageSize(10)
					.AddChoices(new[] {
						"Add a Hall", "Delete a hall", "Show hall's detail", "Back"
					})
					.HighlightStyle(new Style(Color.PaleGreen3)));

			switch (selection)
			{
				case "Add a Hall":
                    _viewFactory.GetService(ViewConstant.AddHall)?.Render(cinema);
					break;
                case "Delete a hall":
					string hallNameToDelete = AnsiConsole.Ask<string>(" -> Enter hall's name to delete: ");

					Hall? hallToDelete = halls.FirstOrDefault(h => h.Name.RemoveMarks() == hallNameToDelete.RemoveMarks());
					if (hallToDelete == null)
					{
						AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Hall '{hallNameToDelete}' doesn't exist ![/]");
						Console.ReadKey();
						_viewFactory.GetService(ViewConstant.ManageHall)?.Render(cinema.Id, ViewConstant.AdminCinemaDetail);
						return;
					}

					Result result = _cinemaBUS.DeleteHall(hallToDelete);
                    if (result.Success)
					{
						_viewFactory.GetService(ViewConstant.ManageHall)?.Render(cinema.Id, ViewConstant.AdminCinemaDetail, statusMessage: "Delete hall successful !");
					}
					else
					{
                        _viewFactory.GetService(ViewConstant.ManageHall)?.Render(cinema.Id, ViewConstant.AdminCinemaDetail, statusMessage: $"Error !, delete failed");
                    }

                    break;
                case "Show hall's detail":
                    string hallName = AnsiConsole.Ask<string>(" -> Enter hall's name to view detail: ");

                    Hall? hallToView = halls.FirstOrDefault(h => h.Name.RemoveMarks() == hallName.RemoveMarks());
                    if (hallToView == null)
                    {
                        AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Hall '{hallName}' doesn't exist ![/]");
                        Console.ReadKey();
                        _viewFactory.GetService(ViewConstant.ManageHall)?.Render(cinema.Id, ViewConstant.AdminCinemaDetail);
                        return;
                    }

                    _viewFactory.GetService(ViewConstant.AdminHallDetail)?.Render(hallToView);
                    break;
                case "Back":
					_viewFactory.GetService(previousView ?? ViewConstant.AdminHome)?.Render(cinema.Id);
					break;
			}
        }
    }
}