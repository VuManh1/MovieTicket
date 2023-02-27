using SharedLibrary.DTO;

namespace MovieTicket.SignIn 
{
    public static class SignInManager
    {
        public static User? User { get; set; }
        public static bool IsLogin { get; set; }
    }
}