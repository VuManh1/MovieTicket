using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL;
using DAL.Repositories;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
	public class UserBUS
	{

		private readonly IUnitOfWork _unitOfWork;
		public Result AddBus(User user)
		{
			return _unitOfWork.UserRepository.Add(user);
		}

		public void DeleteBus(string id)
		{
			// _unitOfWork.UserRepository.Delete(id);
		}

		public List<User> GetAllBus()
		{
			return _unitOfWork.UserRepository.GetAll().ToList();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.UserRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
			// _unitOfWork.UserRepository.GetById(id);
		}

		public Result UpdateBus(User entity)
		{
			return _unitOfWork.UserRepository.Update(entity);
		}
	}
}
