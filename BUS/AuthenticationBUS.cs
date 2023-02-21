using SharedLibrary;
using SharedLibrary.DTO;
using BUS.Services;
using BUS.Helpers;
using System.Text.RegularExpressions;
using OtpNet;
using DAL.UnitOfWork;

namespace BUS
{
	public class AuthenticationBUS
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailSender _emailSender;

		public AuthenticationBUS(IUnitOfWork unitOfWork, IEmailSender emailSender)
		{
			_unitOfWork = unitOfWork;
			_emailSender = emailSender;
		}

		public Result Register(User? user)
		{
			if (user == null) return Result.Error();

			if (String.IsNullOrEmpty(user.Email) || String.IsNullOrWhiteSpace(user.Email))
			{
				return Result.Error("Email can not empty !");
			}

			user.Email = user.Email.Trim();
			Regex regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			if (!regex.IsMatch(user.Email))
			{
				return Result.Error("Email not correct !");
			}

			try
			{
				// check if email available ?
				if (_unitOfWork.UserRepository.FirstOrDefault($"Users.email = '{user.Email}'") != null)
				{
					return Result.Error("Email is available !");
				}
			}
			catch
			{
				return Result.NetworkError();
			}

			// check name valid
			if (String.IsNullOrEmpty(user.Name) || String.IsNullOrWhiteSpace(user.Name))
			{
				return Result.Error("Username can not empty !");
			}
			else if (user.Name.Trim().Length > 20)
			{
				return Result.Error("Username must less than 20 charactes !");
			}

			// check password valid
			if (String.IsNullOrEmpty(user.PasswordHash) || String.IsNullOrWhiteSpace(user.PasswordHash))
			{
				return Result.Error("Password can not empty !");
			}
			else if (user.PasswordHash.Length < 6 || user.PasswordHash.Length > 30)
			{
				return Result.Error("Password must be between 6 and 30 charactes !");
			}

			user.Id = Guid.NewGuid().ToString();
			user.ComputeSaltAndHash();
			user.NormalizeName = user.Name.RemoveMarks();
			user.CreateDate = DateOnly.FromDateTime(DateTime.Now);
			user.IsLock = false;
			user.Role = Role.User;

			try
			{
				return _unitOfWork.UserRepository.Add(user);
			}
			catch
			{
				return Result.NetworkError();
			}
		}

		public Result Login(string email, string password)
		{
			User? user;
			try
			{
				user = _unitOfWork.UserRepository.FirstOrDefault($"Users.email = '{email}'");
			}
			catch
			{
				return Result.NetworkError();
			}

			if (user == null)
			{
				return Result.Error("Email or Password is not correct !");
			}

			if (user.PasswordHash != AuthenticationHelper.ComputeHash(password, user.Salt))
			{
				return Result.Error("Email or Password is not correct !");
			}

			if (user.IsLock)
			{
				return Result.Error("Your account is locked !");
			}

			return Result.OK();
		}

		public void SendOTP(string email)
		{
			var bytes = Base32Encoding.ToBytes("JBSWY3DPEHPK3PXP");
			var totp = new Totp(bytes, step: 60);

			var result = totp.ComputeTotp(DateTime.UtcNow);

			_emailSender.SendEmailAsync(email, "Reset password OTP", $"Here is your OTP: {result}.");
		}

		public Result ValidateOTP(string otp)
		{
			var bytes = Base32Encoding.ToBytes("JBSWY3DPEHPK3PXP");
			var totp = new Totp(bytes, step: 60);

			bool isValid = totp.VerifyTotp(otp, out long timeStepMatched, window: null);

			return new Result { Success = isValid };
		}

		public Result ResetPassword(string? email, string newPassword)
		{
			if (email == null)
			{
				return Result.Error("Email not available !");
			}

			try
			{
				User? user = _unitOfWork.UserRepository.FirstOrDefault($"Users.email = '{email}'");

				if (user == null)
				{
					return Result.Error($"Can not find user with email: {email}.");
				}

				user.PasswordHash = newPassword;
				user.ComputeSaltAndHash();

				return _unitOfWork.UserRepository.Update(user);
			}
			catch
			{
				return Result.NetworkError();
			}
		}
	}
}
