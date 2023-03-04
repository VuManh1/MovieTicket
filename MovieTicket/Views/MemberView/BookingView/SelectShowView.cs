using BUS;
using MovieTicket.Factory;
using MovieTicket.SignIn;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Helpers;
using Spectre.Console;

namespace MovieTicket.Views.MemberView.BookingView
{
    public class SelectShowView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly CityBUS _cityBUS;
        private readonly CinemaBUS _cinemaBUS;
        private readonly ShowBUS _showBUS;
        private readonly MovieBUS _movieBUS;

        public SelectShowView(
            IViewFactory viewFactory,
            CityBUS cityBUS,
            CinemaBUS cinemaBUS,
            ShowBUS showBUS,
            MovieBUS movieBUS)
        {
            _viewFactory = viewFactory;
            _cityBUS = cityBUS;
            _cinemaBUS = cinemaBUS;
            _showBUS = showBUS;
            _movieBUS = movieBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.SelectShow;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (!SignInManager.IsLogin || SignInManager.User == null)
            {
                _viewFactory.GetService(ViewConstant.Start)?.Render();
                return;
            }

            Movie? movie = _movieBUS.GetById((int)(model ?? 0));

            if (movie == null)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("movie", ViewConstant.MemberHome);
                return;
            }

            string dateString = GetDate();
            if (dateString == "Go Back")
            {
                _viewFactory.GetService(ViewConstant.MovieDetail)?.Render(model);
                return;
            }

            // Choose date
            DateTime date = DateTime.Parse(dateString);

            User user = SignInManager.User;

            City city;
            if (user.City != null)
            {
                if (AnsiConsole.Confirm($"Use your's city '{user.City.Name}' to find cinema ? : "))
                {
                    city = user.City;
                }
                else
                {
                    city = GetCity();
                }
            }
            else
            {
                city = GetCity();
            }

            // get cinemas in the city
            List<Cinema> cinemas = _cinemaBUS.Find($"CityId = {city.Id}");

            if (cinemas.Count == 0)
            {
                _viewFactory.GetService(ViewConstant.NotFound)?.Render("cinema", ViewConstant.MemberHome);
                return;
            }

            RenderShows(cinemas, date, movie);

            AnsiConsole.MarkupLine(" * Press [dodgerblue2]'C'[/] to select again, [dodgerblue2]'S'[/] to select a show, " +
                "[red]'ESC'[/] to go back");

            while (true)
            {
                var key = ConsoleHelper.InputKey(new List<ConsoleKey>()
                    {
                        ConsoleKey.S,
                        ConsoleKey.C,
                        ConsoleKey.Escape
                    });

                switch (key)
                {
                    case ConsoleKey.Escape:
                        _viewFactory.GetService(ViewConstant.MovieDetail)?.Render(movie.Id);
                        return;
                    case ConsoleKey.S:
                        int showid = AnsiConsole.Ask<int>(" -> Enter show's id (0 to cancel): ");

                        if (showid != 0)
                        {
                            _viewFactory.GetService(ViewConstant.SelectSeat)?.Render(showid);
                            return;
                        }

                        break;
                    case ConsoleKey.C:
                        _viewFactory.GetService(ViewConstant.SelectShow)?.Render(movie.Id);
                        return;
                }
            }
        }

        public void RenderShows(List<Cinema> cinemas, DateTime date, Movie movie)
        {
            foreach (Cinema cinema in cinemas)
            {
                List<Show> shows = _showBUS.Find($"CinemaId = {cinema.Id} AND MovieId = {movie.Id}" +
                    $" AND StartTime > '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                    $" AND Date(StartTime) = '{date.ToString("yyyy-MM-dd")}'");

                Grid showGrid = new();
                shows.Take(10).ToList().ForEach(s =>
                {
                    showGrid.AddColumn();
                });

                int maxRow = (int)Math.Ceiling((double)shows.Count / 10);

                for (int i = 0; i < maxRow; i++)
                {
                    showGrid.AddRow(
                        shows.Skip(i * 10).Take(10).Select(s =>
                        {
                            return new Panel(
                                Align.Center(new Rows(
                                    new Markup(s.StartTime.ToString("HH:mm"))
                                )))
                            {
                                Header = new PanelHeader(s.Id.ToString())
                            };
                        }).ToArray()
                    );
                }

                if (shows.Count == 0)
                {
                    showGrid.AddColumn();
                    showGrid.AddRow("No shows");
                }

                Panel panel = new(
                    Align.Left(showGrid))
                {
                    Border = BoxBorder.Heavy,
                    BorderStyle = new Style(Color.PaleGreen3),
                    Expand = true,
                    Header = new PanelHeader(cinema.Name)
                };
                AnsiConsole.Write(panel);
                Console.WriteLine();
            }
        }

        public City GetCity()
        {
            List<string> cities = _cityBUS.GetAll().Select(c => c.Name).ToList();

            Console.WriteLine();

            // create select city: 
            var city = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a city where you want to book: ")
                    .PageSize(10)
                    .AddChoices(cities)
                    .HighlightStyle(new Style(Color.PaleGreen3)));

            return _cityBUS.FirstOrDefault($"name = '{city}'") ?? new City();
        }

        public string GetDate()
        {
            var today = DateTime.Now;
            List<string> dates = new();
            dates.Add("Go Back");

            for (int i = 0; i < 9; i++)
            {
                dates.Add($"{today.Day}/{today.Month}");
                today = today.AddDays(1);
            }

            Console.WriteLine();

            // create select: 
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[PaleGreen3]Select a day: [/]")
                    .PageSize(10)
                    .AddChoices(dates)
                    .HighlightStyle(new Style(Color.PaleGreen3)));

            return selection;
        }
    }
}
