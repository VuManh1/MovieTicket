using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.CityView
{
    public class ListCityView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly CityBUS _CityBUS;

        private const int CityS_PER_PAGE = 10;

        public ListCityView(IViewFactory viewFactory, CityBUS CityBUS)
        {
            _viewFactory = viewFactory;
            _CityBUS = CityBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<City> Citys = _CityBUS.GetAll();

            if (Citys.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)Citys.Count / CityS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Citys by page
                List<City> CitysToRender = Citys.
                    Skip((page - 1) * CityS_PER_PAGE)
                    .Take(CityS_PER_PAGE).ToList();

                RenderCitys(CitysToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No City :([/]");
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
                    _viewFactory.Render(ViewConstant.AdminListCity, model: page - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.Render(ViewConstant.AdminListCity, model: page + 1);
                    break;
                case ConsoleKey.F:
                    break;
            }
        }

        public void RenderCitys(List<City> Citys)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "City", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name");

            foreach (var City in Citys)
            {
                table.AddRow(
                    City.Id.ToString(),
                    City.Name
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
