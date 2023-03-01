using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class ShowBus
    {
        private readonly IUnitOfWork _unitOfWork;
		public void AddBus(Show Show)
		{
			_unitOfWork.ShowRepository.Add(Show);
		}

		public void DeleteBus(string id)
		{
		}

		public void GetAllBus()
		{
			_unitOfWork.ShowRepository.GetAll();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.ShowRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public void UpdateBus(Show entity)
		{
			_unitOfWork.ShowRepository.Update(entity);
		}
}
}
