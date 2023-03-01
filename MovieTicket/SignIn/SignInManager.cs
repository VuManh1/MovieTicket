using SharedLibrary.DTO;

namespace MovieTicket.SignIn 
{
    public static class SignInManager
    {
        public static User? User { get; set; }
        public static bool IsLogin { get; set; }

        public static void SignIn(User user)
        {
            User = user;
            IsLogin = true;
        }

        public static void Logout()
        {
            User = null;
            IsLogin = false;
        }
    }
}