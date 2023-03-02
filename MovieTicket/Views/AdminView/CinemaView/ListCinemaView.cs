using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;
#pragma warning disable

namespace MovieTicket.Views.AdminView.CinemaView
{
    public class ListCinemaView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly CinemaBus _CinemaBUS;

        private const int CinemaS_PER_PAGE = 10;

        public ListCinemaView(IViewFactory viewFactory, CinemaBus CinemaBUS)
        {
            _viewFactory = viewFactory;
            _CinemaBUS = CinemaBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Cinema> Cinemas = _CinemaBUS.GetAllBus();

            if (Cinemas.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)Cinemas.Count / CinemaS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Cinemas by page
                List<Cinema> CinemasToRender = Cinemas.
                    Skip((page - 1) * CinemaS_PER_PAGE)
                    .Take(CinemaS_PER_PAGE).ToList();

                RenderCinemas(CinemasToRender);

                PagingModel pagingModel = new()
                {
                    CurrentPage = page,
                    NumberOfPage = numberOfPage
                };

                // render pagination
                _viewFactory.GetService(ViewConstant.Paging)?.Render(model: pagingModel);
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No Cinema :([/]");
            }

            AnsiConsole.MarkupLine("");
            var key = ConsoleHelper.InputKey(new List<ConsoleKey>()
                {
                    ConsoleKey.LeftArrow,
                    ConsoleKey.RightArrow
                });
            
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    _viewFactory.Render(ViewConstant.AdminListCinema, model: page - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.Render(ViewConstant.AdminListCinema, model: page + 1);
                    break;
                case ConsoleKey.F:
                    break;
            }
        }

        public void RenderCinemas(List<Cinema> Cinemas)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Cinema", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name", "HallCount", "City Name");

            foreach (var Cinema in Cinemas)
            {
                table.AddRow(
                    Cinema.Id.ToString(),
                    Cinema.Name,
                    Cinema.HallCount.ToString(),
                    Cinema.City.Name ?? ""
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
