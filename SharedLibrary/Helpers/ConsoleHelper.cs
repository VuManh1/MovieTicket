using SharedLibrary.DTO;
using System.Text.RegularExpressions;

namespace SharedLibrary.Helpers
{
	public static class ConsoleHelper
	{
		public static string InputEmail(string? msg = null)
		{
			Console.Write(msg ?? " -> Enter email: ");

			string email = Console.ReadLine()?.Trim() ?? "";
			Regex regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			
			while (!regex.IsMatch(email))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Email invalid !");
				Console.ForegroundColor = ConsoleColor.White;

				Console.Write(msg ?? " -> Enter email: ");
				email = Console.ReadLine()?.Trim() ?? "";
			}

			return email;
		}

		public static string InputUserName(string? msg = null)
		{
			Console.Write(msg ?? " -> Enter username: ");

			string name = Console.ReadLine()?.Trim() ?? "";

			while (name.Length == 0 || name.Length > 20)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Username must less than 20 charactes and can not empty !");
				Console.ForegroundColor = ConsoleColor.White;

				Console.Write(msg ?? " -> Enter username: ");
				name = Console.ReadLine()?.Trim() ?? "";
			}

			return name;
		}

		public static string InputPhoneNumber(string? msg = null)
		{
			Console.Write(msg ?? " -> Enter phone number: ");

			string number = Console.ReadLine()?.Trim() ?? "";

			Regex regex = new(@"\(?\d{3}\)?[-\.]? *\d{3}[-\.]? *[-\.]?\d{4}");

			while (!regex.IsMatch(number))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Invalid phone number !");
				Console.ForegroundColor = ConsoleColor.White;

				Console.Write(msg ?? " -> Enter phone number: ");
				number = Console.ReadLine()?.Trim() ?? "";
			}

			return number;
		}

        public static ConsoleKey InputKey(IEnumerable<ConsoleKey> keys)
        {
			var keyinfo = Console.ReadKey(true);
			ConsoleKey key = keyinfo.Key;

            while (!keys.Contains(key))
            {
                keyinfo = Console.ReadKey(true);
                key = keyinfo.Key;
            }

            return key;
        }
    }
}
