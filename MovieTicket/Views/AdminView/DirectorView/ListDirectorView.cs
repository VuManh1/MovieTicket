﻿using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.DirectorView
{
    public class ListDirectorView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly DirectorBUS _directorBUS;

        private const int DIRECTORS_PER_PAGE = 10;

        public ListDirectorView(IViewFactory viewFactory, DirectorBUS directorBUS)
        {
            _viewFactory = viewFactory;
            _directorBUS = directorBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<Director> directors = _directorBUS.GetAll();

            if (directors.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)directors.Count / DIRECTORS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Directors by page
                List<Director> directorsToRender = directors.
                    Skip((page - 1) * DIRECTORS_PER_PAGE)
                    .Take(DIRECTORS_PER_PAGE).ToList();

                RenderDirectors(directorsToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No Director :([/]");
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
                    _viewFactory.Render(ViewConstant.AdminListDirector, model: page - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.Render(ViewConstant.AdminListDirector, model: page + 1);
                    break;
                case ConsoleKey.F:
                    break;
            }
        }

        public void RenderDirectors(List<Director> directors)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "Director", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name", "About");

            foreach (var director in directors)
            {
                table.AddRow(
                    director.Id.ToString(),
                    director.Name,
                    director.About ?? ""
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
