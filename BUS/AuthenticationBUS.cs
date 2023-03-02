using SharedLibrary;
using SharedLibrary.DTO;
using BUS.Services;
using SharedLibrary.Helpers;
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

			try
			{
				// check if email available ?
				if (_unitOfWork.UserRepository.FirstOrDefault($"email = '{user.Email}'") != null)
				{
					return Result.Error("Email is available !");
				}
			}
			catch
			{
				return Result.NetworkError();
			}

			user.ComputeSaltAndHash();
			user.NormalizeName = user.Name.RemoveMarks();
			user.CreateDate = DateOnly.FromDateTime(DateTime.Now);
			user.IsLock = false;
			user.Role = Role.Member;

			try
			{
				return _unitOfWork.UserRepository.Create(user);
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
				user = _unitOfWork.UserRepository.FirstOrDefault($"email = '{email}'");
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
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

			return Result.OK(user);
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
