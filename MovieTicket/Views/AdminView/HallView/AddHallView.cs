using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Helpers;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.HallView
{
    public class AddHallView : IViewRender
    {
		private readonly CinemaBUS _cinemaBUS;
        private readonly IViewFactory _viewFactory;

        public AddHallView(CinemaBUS cinemaBUS, IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _cinemaBUS = cinemaBUS;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AddHall;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            Cinema? cinema = (Cinema?)model;
            if (cinema == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("cinema", ViewConstant.AdminHome);
                return;
            }

            AnsiConsole.MarkupLine($"\n[{ColorConstant.Primary}]Add a hall for cinema: {cinema.Name} \n[/]");

            List<Hall> halls = _cinemaBUS.GetHalls(cinema);

            Hall hall = new()
            {
                Cinema = cinema,
                Name = AnsiConsole.Ask<string>(" -> Enter hall's name: ")
            };

            // check if exist any hall have the same name in this cinema
            while (halls.Any(h => h.Name.RemoveMarks() == hall.Name.RemoveMarks()))
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Already have a hall name '{hall.Name}'.[/]");

                if (!AnsiConsole.Confirm("Continue ? : "))
                {
                    _viewFactory.GetService(ViewConstant.ManageHall)?.Render(cinema.Id, ViewConstant.AdminCinemaDetail);
                    return;
                }

                hall.Name = AnsiConsole.Ask<string>(" -> Enter hall's name: ");
            }

            hall.Width = AnsiConsole.Ask<int>(" -> Enter hall width (EX: 12 corresponds to 12 seat horizontally): ");
            hall.Height = AnsiConsole.Ask<int>(" -> Enter hall height (EX: 12 corresponds to 12 seat vertically): ");

            Result result = _cinemaBUS.CreateHall(hall);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add Hall successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.GetService(ViewConstant.ManageHall)?.Render(cinema.Id);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.GetService(ViewConstant.ManageHall)?.Render(cinema.Id);
                    return;
                }

                _viewFactory.GetService(ViewConstant.AddHall)?.Render(cinema);
            }
        }
    }
}