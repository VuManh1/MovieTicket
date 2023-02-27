using System.Text.RegularExpressions;

namespace SharedLibrary.Helpers
{
	public class ValidationHelper
	{
		public static bool CheckPassword(string password)
			=> password.Length >= 6 && password.Length <= 30;

		public static bool CheckEmail(string email)
		{
			Regex regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

			return regex.IsMatch(email);
		}
	}
}
