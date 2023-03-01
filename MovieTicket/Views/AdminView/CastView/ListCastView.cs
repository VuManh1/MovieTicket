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
        private readonly CastBus _CastBUS;

        private const int CastS_PER_PAGE = 10;

        public ListCastView(IViewFactory viewFactory, CastBus CastBUS)
        {
            _viewFactory = viewFactory;
            _CastBUS = CastBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Cast> Casts = _CastBUS.GetAllBus();

            if (Casts.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)Casts.Count / CastS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Casts by page
                List<Cast> CastsToRender = Casts.
                    Skip((page - 1) * CastS_PER_PAGE)
                    .Take(CastS_PER_PAGE).ToList();

                RenderCasts(CastsToRender);

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

        public void RenderCasts(List<Cast> Casts)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Cast", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name", "About");

            foreach (var Cast in Casts)
            {
                table.AddRow(
                    Cast.Id.ToString(),
                    Cast.Name,
                    Cast.About ?? ""
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
