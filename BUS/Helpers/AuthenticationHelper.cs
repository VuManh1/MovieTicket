using SharedLibrary.DTO;
using System.Security.Cryptography;

namespace BUS.Helpers
{
	public static class AuthenticationHelper
	{
		public static void ComputeSaltAndHash(this User user)
		{
			byte[] salt = GenerateSalt();
			user.Salt = Convert.ToBase64String(salt);
			user.PasswordHash = ComputeHash(user.PasswordHash, user.Salt);
		}

		public static byte[] GenerateSalt()
		{
			var rng = RandomNumberGenerator.Create();
			byte[] salt = new byte[24];
			rng.GetBytes(salt);

			return salt;
		}

		public static string ComputeHash(string password, string salt)
		{
			using var hashGenerator = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt));
			hashGenerator.IterationCount = 10101;
			byte[] bytes = hashGenerator.GetBytes(24);

			return Convert.ToBase64String(bytes);
		}
	}
}

