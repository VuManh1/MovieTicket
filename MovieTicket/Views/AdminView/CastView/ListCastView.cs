using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.CastView
{
    public class ListCastView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly CastBUS _castBUS;

        private const int CASTS_PER_PAGE = 10;

        public ListCastView(IViewFactory viewFactory, CastBUS castBUS)
        {
            _viewFactory = viewFactory;
            _castBUS = castBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Cast> casts = _castBUS.GetAll();

            if (casts.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)casts.Count / CASTS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Casts by page
                List<Cast> castsToRender = casts.
                    Skip((page - 1) * CASTS_PER_PAGE)
                    .Take(CASTS_PER_PAGE).ToList();

                RenderCasts(castsToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No Cast :([/]");
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
                    _viewFactory.Render(ViewConstant.AdminListCast, model: page - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.Render(ViewConstant.AdminListCast, model: page + 1);
                    break;
                case ConsoleKey.F:
                    break;
            }
        }

        public void RenderCasts(List<Cast> casts)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Cast", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name", "About");

            foreach (var cast in casts)
            {
                table.AddRow(
                    cast.Id.ToString(),
                    cast.Name,
                    cast.About ?? ""
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
