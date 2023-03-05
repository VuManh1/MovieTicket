using BUS;
using MovieTicket.Factory;
using MovieTicket.SignIn;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Helpers;
using SharedLibrary.Models;
using Spectre.Console;

namespace MovieTicket.Views.MemberView
{
    public class HomeView : IViewRender
    {
		private readonly IViewFactory _viewFactory;
		private readonly MovieBUS _movieBUS;

        public HomeView(IViewFactory viewFactory, MovieBUS movieBUS)
		{
			_viewFactory = viewFactory;
            _movieBUS = movieBUS;
		}

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.MemberHome;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            List<Movie> playingMovies = _movieBUS.GetByStatus(MovieStatus.Playing).Take(10).ToList();
            List<Movie> comingMovies = _movieBUS.GetByStatus(MovieStatus.Coming).Take(10).ToList();
            RenderLayout(playingMovies, comingMovies);

            if (SignInManager.IsLogin)
            {
                AnsiConsole.MarkupLine($" * Press [{ColorConstant.Primary}]'P'[/] to view my profile");
                AnsiConsole.MarkupLine(" * Press [dodgerblue2]'C'[/] to choose a movie, [dodgerblue2]'F'[/] to move to search page, " +
                    $"[red]'ESC'[/] to logout");
            }
            else
            {
                AnsiConsole.MarkupLine(" * Press [dodgerblue2]'C'[/] to choose a movie, [dodgerblue2]'F'[/] to move to search page, " +
                    $"[red]'ESC'[/] to go back");
            }

            while (true)
            {
                var key = ConsoleHelper.InputKey(new List<ConsoleKey>()
                    {
                        ConsoleKey.F,
                        ConsoleKey.C,
                        ConsoleKey.P,
                        ConsoleKey.A,
                        ConsoleKey.S,
                        ConsoleKey.Escape
                    });

                switch (key)
                {
                    case ConsoleKey.Escape:
                        if (SignInManager.IsLogin)
                            SignInManager.Logout();

                        _viewFactory.GetService(ViewConstant.Start)?.Render();
                        return;
                    case ConsoleKey.F:
                        _viewFactory.GetService(ViewConstant.MovieList)?.Render();
                        return;
                    case ConsoleKey.A:
                        _viewFactory.GetService(ViewConstant.MovieList)?.Render(new SearchModel
                        {
                            SearchValue = "<Playing>"
                        });
                        return;
                    case ConsoleKey.S:
                        _viewFactory.GetService(ViewConstant.MovieList)?.Render(new SearchModel
                        {
                            SearchValue = "<Coming>"
                        });
                        return;
                    case ConsoleKey.P:
                        if (SignInManager.IsLogin && SignInManager.User != null)
                        {
                            _viewFactory.GetService(ViewConstant.MemberDetail)?.Render();
                            return;
                        }
                        break;
                    case ConsoleKey.C:
                        int movieid = AnsiConsole.Ask<int>(" -> Enter movie's id to view movie detail (0 to cancel): ");

                        if (movieid != 0)
                        {
                            _viewFactory.GetService(ViewConstant.MovieDetail)?.Render(movieid);
                            return;
                        }

                        break;
                }
            }
        }

        public void RenderLayout(List<Movie> playingMovies, List<Movie> comingMovies)
        {
            Grid playingGrid = new();

            playingMovies.Take(5).ToList().ForEach(m =>
            {
                playingGrid.AddColumn();
            });

            if (playingMovies.Count > 0)
            {
                playingGrid.AddRow(
                    playingMovies.Take(5).Select(m =>
                    {
                        string movieName = m.Name.Length > 25 ? String.Join("", m.Name.Take(25)) + "..." : m.Name;

                        return new Panel(
                            Align.Center(new Rows(
                                new Markup($"[{ColorConstant.Primary}]{movieName}[/]"),
                                new Text($"{m.Length} minutes")
                            ))
                        )
                        {
                            Header = new PanelHeader(m.Id.ToString())
                        };
                    }).ToArray()
                );

                playingGrid.AddRow(
                    playingMovies.Skip(5).Take(5).Select(m =>
                    {
                        string movieName = m.Name.Length > 25 ? String.Join("", m.Name.Take(25)) + "..." : m.Name;

                        return new Panel(
                            Align.Center(new Rows(
                                new Markup($"[{ColorConstant.Primary}]{movieName}[/]"),
                                new Text($"{m.Length} minutes")
                            )))
                        {
                            Header = new PanelHeader(m.Id.ToString())
                        };
                    }).ToArray()
                );
            }
            else
            {
                playingGrid.AddColumn();
                playingGrid.AddRow("No movies");
            }

            Grid comingGrid = new();
            comingMovies.Take(5).ToList().ForEach(m =>
            {
                comingGrid.AddColumn();
            });

            if (comingMovies.Count > 0) 
            { 
                comingGrid.AddRow(
                    comingMovies.Take(5).Select(m =>
                    {
                        string movieName = m.Name.Length > 25 ? String.Join("", m.Name.Take(25)) + "..." : m.Name;

                        return new Panel(
                            Align.Center(new Rows(
                                new Markup($"[Gold3_1]{movieName}[/]"),
                                new Text($"{m.Length} minutes")
                            )))
                        {
                            Header = new PanelHeader(m.Id.ToString())
                        };
                    }).ToArray()
                );

                comingGrid.AddRow(
                    comingMovies.Skip(5).Take(5).Select(m =>
                    {
                        string movieName = m.Name.Length > 25 ? String.Join("", m.Name.Take(25)) + "..." : m.Name;

                        return new Panel(
                            Align.Center(new Rows(
                                new Markup($"[Gold3_1]{movieName}[/]"),
                                new Text($"{m.Length} minutes")
                            )))
                        {
                            Header = new PanelHeader(m.Id.ToString())
                        };
                    }).ToArray()
                );
            }
            else
            {
                comingGrid.AddColumn();
                comingGrid.AddRow("No movies");
            }

            var playingMoviePanel = new Panel(
                Align.Center(playingGrid))
            {
                Border = BoxBorder.Heavy,
                BorderStyle = new Style(Color.PaleGreen3),
                Expand = true
            };
            AnsiConsole.MarkupLine($"[{ColorConstant.Primary}]Playing Movies[/]   (Press [PaleGreen3]'A'[/] to view all)");
            AnsiConsole.Write(playingMoviePanel);

            Console.WriteLine();

            var comingMoviePanel = new Panel(
                Align.Center(comingGrid))
            {
                Border = BoxBorder.Heavy,
                BorderStyle = new Style(Color.Gold3_1),
                Expand = true
            };
            AnsiConsole.MarkupLine($"[Gold3_1]Upcoming Movies[/]   (Press [Gold3_1]'S'[/] to view all)");
            AnsiConsole.Write(comingMoviePanel);
        }
    }
}