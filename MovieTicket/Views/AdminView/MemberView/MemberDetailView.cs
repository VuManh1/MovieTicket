using BUS;
using MovieTicket.Factory;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using Spectre.Console;

namespace MovieTicket.Views.AdminView.MemberView
{
    public class MemberDetailView : IViewRender
    {
		private readonly IViewFactory _viewFactory;
		private readonly UserBUS _userBUS;

        public MemberDetailView(IViewFactory viewFactory, UserBUS userBUS)
		{
			_viewFactory = viewFactory;
            _userBUS = userBUS;
		}

        public void Render(string? statusMessage = null, object? model = null)
        {
            _viewFactory.GetService(ViewConstant.LoginInfo)?.Render();

            if (model == null)
            {
                _viewFactory.Render(ViewConstant.NotFound, "user", ViewConstant.AdminListMember);
                return;
            }

            int userid = (int)model;

            User? user = _userBUS.GetById(userid);

            if (user == null)
            {
                _viewFactory.Render(ViewConstant.NotFound, "user", ViewConstant.AdminListMember);
                return;
            }

            // render movie detail
            RenderUserInfo(user);

            // check status message
            if (statusMessage != null)
            {
                if (statusMessage.StartsWith("Error"))
                    AnsiConsole.MarkupLine($"[{ColorConstant.Error}]{statusMessage}[/]\n");
                else
                    AnsiConsole.MarkupLine($"[{ColorConstant.Success}]{statusMessage}[/]\n");
            }

            // create select: 
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose : ")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Go Back", user.IsLock ? "Unlock Member" : "Lock Member", "Delete this Member",
                    })
                    .HighlightStyle(new Style(Color.PaleGreen3)));


            switch (selection)
            {
                case "Go Back":
                    _viewFactory.Render(ViewConstant.AdminListMember);
                    return;
                case "Lock Member":
                    user.IsLock = true;
                    Result lockResult = _userBUS.Update(user);

                    if (lockResult.Success)
                        _viewFactory.Render(ViewConstant.AdminMemberDetail, "Member locked !", user.Id);
                    else
                        _viewFactory.Render(ViewConstant.AdminMemberDetail, "Error !, " + lockResult.Message, user.Id);

                    return;
                case "Unlock Member":
                    user.IsLock = false;
                    Result unlockResult = _userBUS.Update(user);

                    if (unlockResult.Success)
                        _viewFactory.Render(ViewConstant.AdminMemberDetail, "Member unlocked !", user.Id);
                    else
                        _viewFactory.Render(ViewConstant.AdminMemberDetail, "Error !, " + unlockResult.Message, user.Id);

                    return;
                case "Delete this Member":
                    if (!AnsiConsole.Confirm("Delete this Member ? : "))
                    {
                        _viewFactory.Render(ViewConstant.AdminMemberDetail, model: user.Id);
                        return;
                    }

                    Result deleteResult = _userBUS.Delete(user);

                    if (deleteResult.Success)
                        _viewFactory.Render(ViewConstant.AdminListMember);
                    else
                        _viewFactory.Render(ViewConstant.AdminMemberDetail, "Error !, " + deleteResult.Message, user.Id);

                    return;
            }
        }

        public void RenderUserInfo(User user)
        {
            Rows rows = new(
                new Markup($"[{ColorConstant.Primary}]Id: [/]{user.Id}"),
                new Markup($"[{ColorConstant.Primary}]Name: [/]{user.Name}"),
                new Markup($"[{ColorConstant.Primary}]Email: [/]{user.Email}"),
                new Markup($"[{ColorConstant.Primary}]Phone number: [/]{user.PhoneNumber}"),
                new Markup($"[{ColorConstant.Primary}]Create Date: [/]{user.CreateDate}"),
                new Markup($"[{ColorConstant.Primary}]Is Lock: [/]{user.IsLock}"),
                new Markup($"[{ColorConstant.Primary}]Role: [/]{user.Role.ToString()}"),
                new Markup($"[{ColorConstant.Primary}]City: [/]{user.City?.Name}")
            );

            var panel = new Panel(
                Align.Left(rows))
            {
                Border = BoxBorder.Heavy,
                BorderStyle = new Style(Color.PaleGreen3),
                Expand = true,
                Header = new PanelHeader("User Detail")
            };
            panel.Header.Centered();

            AnsiConsole.Write(panel);
        }
    }
}