using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;

namespace MovieTicket.Views.AdminView.MemberView
{
    public class ListMemberView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly UserBUS _userBUS;

        private const int USERS_PER_PAGE = 10;

        public ListMemberView(IViewFactory viewFactory, UserBUS userBUS)
        {
            _viewFactory = viewFactory;
            _userBUS = userBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.AdminListMember;

            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            SearchModel searchModel = model != null ? (SearchModel)model : new SearchModel() { Page = 1 };

            int page = searchModel.Page;
            if (page <= 0) page = 1;

            List<User> users;
            if (searchModel.SearchValue != null)
            {
                AnsiConsole.Markup($"[{ColorConstant.Info}]Search for '{searchModel.SearchValue}'[/]\n");
                users = _userBUS.FindMember(searchModel.SearchValue);
            }
            else
                users = _userBUS.GetAllMember();


            if (users.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)users.Count / USERS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get movies by page
                List<User> usersToRender = users.
                    Skip((page - 1) * USERS_PER_PAGE)
                    .Take(USERS_PER_PAGE).ToList();

                RenderUsers(usersToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No user :([/]\n");
            }

            AnsiConsole.MarkupLine(" * Press [dodgerblue2]'C'[/] to choose a user, [dodgerblue2]'F'[/] to search users, " +
                "[red]'ESCAPE'[/] to go back");
            var key = ConsoleHelper.InputKey(new List<ConsoleKey>()
                {
                    ConsoleKey.LeftArrow,
                    ConsoleKey.RightArrow,
                    ConsoleKey.F,
                    ConsoleKey.C,
                    ConsoleKey.Escape
                });

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    _viewFactory.GetService(ViewConstant.AdminListMember)?.Render(new SearchModel()
                    {
                        Page = page - 1,
                        SearchValue = searchModel.SearchValue,
                    }, previousView);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.GetService(ViewConstant.AdminListMember)?.Render(new SearchModel()
                    {
                        Page = page + 1,
                        SearchValue = searchModel.SearchValue
                    }, previousView);
                    break;
                case ConsoleKey.F:
                    searchModel.SearchValue = AnsiConsole.Ask<string>(" -> Enter user's name to search: ");

                    _viewFactory.GetService(ViewConstant.AdminListMember)?.Render(new SearchModel()
                    {
                        Page = 1,
                        SearchValue = searchModel.SearchValue
                    }, ViewConstant.AdminListMember);
                    break;
                case ConsoleKey.C:
                    int id = AnsiConsole.Ask<int>(" -> Enter user's id (0 to cancel): ");

                    if (id == 0)
                    {
                        _viewFactory.GetService(ViewConstant.AdminListMember)?.Render(new SearchModel()
                        {
                            Page = page,
                            SearchValue = searchModel.SearchValue
                        }, previousView);
                        return;
                    }

                    _viewFactory.GetService(ViewConstant.AdminMemberDetail)?.Render(id);
                    break;
                case ConsoleKey.Escape:
                    _viewFactory.GetService(previousView ?? ViewConstant.AdminHome)?.Render();
                    break;
            }
        }

        public void RenderUsers(List<User> users)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "User", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Name", "Email", "Join Date", "City");

            foreach (var user in users)
            {
                table.AddRow(
                    user.Id.ToString(),
                    user.Name,
                    user.Email,
                    user.CreateDate?.ToString() ?? "",
                    user.City?.Name ?? ""
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
