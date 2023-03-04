using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
using SharedLibrary.Helpers;

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

        public List<User> Find(string filter)
        {
            return _unitOfWork.UserRepository.Find(filter).ToList();
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
