using MovieTicket.Factory;
using SharedLibrary.Constants;
using Spectre.Console;
using SharedLibrary.DTO;
using BUS;
using SharedLibrary.Models;
using SharedLibrary.Helpers;
#pragma warning disable


namespace MovieTicket.Views.AdminView.UserView
{
    public class ListUserView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly UserBUS _UserBUS;

        private const int UserS_PER_PAGE = 10;

        public ListUserView(IViewFactory viewFactory, UserBUS UserBUS)
        {
            _viewFactory = viewFactory;
            _UserBUS = UserBUS;
        }

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            int page = model != null ? (int)model : 1; 
            
            if (page <= 0) page = 1;

            List<User> Users = _UserBUS.GetAllBus();

            if (Users.Count > 0)
            {
                int numberOfPage = (int)Math.Ceiling((double)Users.Count / UserS_PER_PAGE);

                if (page > numberOfPage) page = numberOfPage;

                // get Users by page
                List<User> UsersToRender = Users.
                    Skip((page - 1) * UserS_PER_PAGE)
                    .Take(UserS_PER_PAGE).ToList();

                RenderUsers(UsersToRender);

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
                AnsiConsole.MarkupLine($"[{ColorConstant.Error}]No User :([/]");
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
                    _viewFactory.Render(ViewConstant.AdminListUser, model: page - 1);
                    break;
                case ConsoleKey.RightArrow:
                    _viewFactory.Render(ViewConstant.AdminListUser, model: page + 1);
                    break;
                case ConsoleKey.F:
                    break;
            }
        }

        public void RenderUsers(List<User> Users)
        {
            Table table = new()
            {
                Title = new TableTitle(
                    "User", 
                    new Style(Color.PaleGreen3)),
                Expand = true
            };
            table.AddColumns("Id", "Hall ID", "UserType", "UserNumber","Price");

            foreach (var User in Users)
            {
                table.AddRow(
                    User.Id.ToString(),
                    User.Name,
                    User.NormalizeName,
                    User.Email,
                    User.PhoneNumber.ToString(),
                    User.City.Id.ToString(),
                    User.Salt,
                    User.Role.ToString(),
                    User.CreateDate.ToString(),
                    User.IsLock.ToString()
                );
            }

            table.Border(TableBorder.Heavy);
            AnsiConsole.Write(table);
        }
    }
}
