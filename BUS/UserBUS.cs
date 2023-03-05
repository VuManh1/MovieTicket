using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
using SharedLibrary.Helpers;
using SharedLibrary.Models;

namespace BUS
{
	public class UserBUS
	{

		private readonly IUnitOfWork _unitOfWork;

        public UserBUS(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

		public Result Delete(User user)
		{
			try
			{
				return _unitOfWork.UserRepository.Delete(user);
			}
			catch
			{
				return Result.NetworkError();
			}
		}

        public List<User> GetAllMember()
        {
            try
            {
                return _unitOfWork.UserRepository.Find($"role = 'Member'").ToList();
            }
            catch
            {
                return new List<User>();
            }
        }

        public List<User> FindMember(string filter)
        {
			try
			{
				return _unitOfWork.UserRepository.Find(
					$"NormalizeName like '%{filter}%' AND" +
					$" role = 'Member'").ToList();
			}
			catch
			{
				return new List<User>();
			}
        }

		public User? GetById(int id)
		{
			try
			{
				return _unitOfWork.UserRepository.GetById(id);
			}
			catch
			{
				return null;
			}
		}

		public Result Update(User entity)
		{
			entity.NormalizeName = entity.Name.RemoveMarks();

			try
			{
				return _unitOfWork.UserRepository.Update(entity);
			}
			catch
			{
				return Result.NetworkError();
			}
		}
	}
}
