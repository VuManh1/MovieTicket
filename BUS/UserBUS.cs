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
		public void AddBus(User user)
		{
			_unitOfWork.UserRepository.Add(user);
		}

		public void DeleteBus(string id)
		{
		}

		public void GetAllBus()
		{
			_unitOfWork.UserRepository.GetAll();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.UserRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public void UpdateBus(User entity)
		{
			_unitOfWork.UserRepository.Update(entity);
		}
	}
}
