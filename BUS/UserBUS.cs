using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;

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

		public List<User> GetAll()
		{
            try
            {
                return _unitOfWork.UserRepository.GetAll().ToList();
            }
            catch
            {
                return new List<User>();
            }
        }

        public List<User> Find(string filter)
        {
            return _unitOfWork.UserRepository.Find(filter).ToList();
        }

        public void FirstOrDefault(string filter)
		{
			_unitOfWork.UserRepository.FirstOrDefault(filter);
		}

		public User? GetById(int id)
		{
	        return _unitOfWork.UserRepository.GetById(id);
		}

		public Result Update(User entity)
		{
			return _unitOfWork.UserRepository.Update(entity);
		}
	}
}
