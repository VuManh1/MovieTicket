using BUS;
using MovieTicket.Factory;
using MovieTicket.SignIn;
using SharedLibrary;
using SharedLibrary.Constants;
using SharedLibrary.DTO;
using SharedLibrary.Helpers;
using Spectre.Console;

namespace MovieTicket.Views.MemberView
{
    public class MemberDetailView : IViewRender
    {
        private readonly IViewFactory _viewFactory;
        private readonly UserBUS _userBUS;
        private readonly CityBUS _cityBUS;

        public MemberDetailView(IViewFactory viewFactory, UserBUS userBUS, CityBUS cityBUS)
        {
            _viewFactory = viewFactory;
            _userBUS = userBUS;
            _cityBUS = cityBUS;
        }

        public void Render(object? model = null, string? previousView = null, string? statusMessage = null)
        {
            Console.Clear();
            Console.Title = ViewConstant.MemberDetail;

            if (!SignInManager.IsLogin || SignInManager.User == null)
            {
                _viewFactory.GetService(ViewConstant.Start)?.Render();
                return;
            }

            User member = SignInManager.User;

            RenderUserInfo(member);

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a action: ")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Go Back", "View my bookings", "Change username",
                        "Change phonenumber", "Change city"
                    })
                    .HighlightStyle(new Style(Color.PaleGreen3)));

            switch (selection)
            {
                case "Go Back":
                    _viewFactory.GetService(ViewConstant.MemberHome)?.Render();
                    return;
                case "View my bookings":
                    _viewFactory.GetService(ViewConstant.ListBooking)?.Render();
                    return;
                case "Change username":
                    member.Name = AnsiConsole.Ask<string>(" -> Change username: ");
                    break;
                case "Change phonenumber":
                    member.PhoneNumber = AnsiConsole.Ask<string>(" -> Change phone number (0 to remove phonenumber): ");
                    // check phone
                    while (!ValidationHelper.CheckPhoneNumber(member.PhoneNumber) && member.PhoneNumber != "0")
                    {
                        AnsiConsole.MarkupLine($"[{ColorConstant.Error}]Invalid phone number ![/]");
                        member.PhoneNumber = AnsiConsole.Ask<string>(" -> Enter phone number (0 to remove phonenumber): ");
                    }

                    if (member.PhoneNumber == "0")
                    {
                        member.PhoneNumber = null;
                    }

                    break;
                case "Change city":
                    member.City = GetCity();
                    break;
            }

            Result result = _userBUS.Update(member);

            if (result.Success)
                _viewFactory.GetService(ViewConstant.MemberDetail)?.Render(member.Id, statusMessage: "Successful change member detail !");
            else
                _viewFactory.GetService(ViewConstant.MemberDetail)?.Render(member.Id, statusMessage: "Error !, " + result.Message);

        }

        public void RenderUserInfo(User user)
        {
            Rows rows = new(
                new Markup($"[{ColorConstant.Primary}]Id: [/]{user.Id}"),
                new Markup($"[{ColorConstant.Primary}]Name: [/]{user.Name}"),
                new Markup($"[{ColorConstant.Primary}]Email: [/]{user.Email}"),
                new Markup($"[{ColorConstant.Primary}]Phone number: [/]{user.PhoneNumber}"),
                new Markup($"[{ColorConstant.Primary}]Join Date: [/]{user.CreateDate}"),
                new Markup($"[{ColorConstant.Primary}]Role: [/]{user.Role.ToString()}"),
                new Markup($"[{ColorConstant.Primary}]City: [/]{user.City?.Name}")
            );

            var panel = new Panel(
                Align.Left(rows))
            {
                Border = BoxBorder.Heavy,
                BorderStyle = new Style(Color.PaleGreen3),
                Expand = true,
                Header = new PanelHeader("User Information")
            };
            panel.Header.Centered();

            AnsiConsole.Write(panel);
        }

        public City? GetCity()
        {
            List<string> cities = _cityBUS.GetAll().Select(c => c.Name).ToList();
            cities.Insert(0, "Skip");

            Console.WriteLine();

            // create select city: 
            var city = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\nChange city: ")
                    .PageSize(10)
                    .AddChoices(cities)
                    .HighlightStyle(new Style(Color.PaleGreen3)));

            return city != "Skip" ? _cityBUS.FirstOrDefault($"name = '{city}'") : null;
        }
    }
}
