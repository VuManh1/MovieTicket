using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.ShowView
{
    public class AddShowView : IViewRender
    {
		private readonly ShowBUS _showBUS;
		private readonly MovieBUS _movieBUS;
		private readonly CinemaBUS _cinemaBUS;
        private readonly IViewFactory _viewFactory;

        public AddShowView(
            ShowBUS showBUS, 
            MovieBUS movieBUS, 
            CinemaBUS cinemaBUS, 
            IViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
            _showBUS = showBUS;
            _movieBUS = movieBUS;
            _cinemaBUS = cinemaBUS;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AddShow;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Add Show \n[/]");

            // get movie
            int movieid = AnsiConsole.Ask<int>(" -> Enter movie's id: ");
            Movie? movie = _movieBUS.GetById(movieid);

            while (movie == null)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Can not find movie with id = {movieid}[/]");

                if (!AnsiConsole.Confirm("Continue ? : "))
                {
                    _viewFactory.GetService(ViewConstant.AdminHome)?.Render();
                    return;
                }

                movieid = AnsiConsole.Ask<int>(" -> Enter movie's id: ");
                movie = _movieBUS.GetById(movieid);
            }
            
            // get cinema
            int cinemaid = AnsiConsole.Ask<int>(" -> Enter cinema's id: ");
            Cinema? cinema = _cinemaBUS.GetById(cinemaid);

            while (cinema == null)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Can not find cinema with id = {cinemaid}[/]");

                if (!AnsiConsole.Confirm("Continue ? : "))
                {
                    _viewFactory.GetService(ViewConstant.AdminHome)?.Render();
                    return;
                }

                cinemaid = AnsiConsole.Ask<int>(" -> Enter cinema's id: ");
                cinema = _cinemaBUS.GetById(cinemaid);
            }

            List<Hall> halls = _cinemaBUS.GetHalls(cinema);
            if (halls.Count <= 0)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Cinema '{cinema.Name}' doesn't have any hall, create a new[/]" +
                    $"[{ColorConstant.Error}] one to create a show.[/]");

                Console.ReadKey();
                _viewFactory.GetService(ViewConstant.AdminHome)?.Render();
                return;
            }

            Show show = new()
            {
                Movie = movie,
                Hall = GetHall(halls),
                StartTime = AnsiConsole.Ask<DateTime>(" -> Enter start time (EX 2-13-2023 14:30:00): ")
            };

            List<Show> shows = _showBUS.Find(show.Hall.Cinema, date:show.StartTime);

            // check if exist a show playing in the same time and same hall
            while (shows.Any(s =>
                DateTime.Compare(show.StartTime, s.StartTime.AddMinutes(-show.Movie.Length)) >= 0 &&
                DateTime.Compare(show.StartTime, s.StartTime.AddMinutes(s.Movie.Length)) <= 0 &&
                s.Hall.Id == show.Hall.Id))
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]This hall already have a show playing in this time[/]");

                if (!AnsiConsole.Confirm("Continue ? : "))
                {
                    _viewFactory.GetService(ViewConstant.ManageShow)?.Render();
                    return;
                }

                show.Hall = GetHall(halls);
                show.StartTime = AnsiConsole.Ask<DateTime>(" -> Enter start time (EX 2-13-2023 14:30:00): ");
            }

            Result result = _showBUS.Create(show);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Success}]Add Show successful ![/], press any key to go back.");
                Console.ReadKey();
               
                _viewFactory.GetService(ViewConstant.ManageShow)?.Render();
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{result.Message}[/]");

                if (!AnsiConsole.Confirm("Add again ? : "))
                {
                    _viewFactory.GetService(ViewConstant.ManageShow)?.Render();
                    return;
                }

                _viewFactory.GetService(ViewConstant.AddShow)?.Render();
            }
        }

        public Hall GetHall(List<Hall> halls)
        {
            List<string> hallsName = halls.Select(h => h.Name).ToList();

            Console.WriteLine();

            // create select hall: 
            var hall = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\nChoose a hall: ")
                    .PageSize(10)
                    .AddChoices(hallsName)
                    .HighlightStyle(new Style(Color.PaleGreen3)));

            return halls.First(h => h.Name == hall);
        }
    }
}