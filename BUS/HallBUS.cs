using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class HallBus
    {
        private readonly IUnitOfWork _unitOfWork;
		public void AddBus(Hall Hall)
		{
			_unitOfWork.HallRepository.Add(Hall);
		}

		public void DeleteBus(string id)
		{
		}

		public void GetAllBus()
		{
			_unitOfWork.HallRepository.GetAll();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.HallRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public void UpdateBus(Hall entity)
		{
			_unitOfWork.HallRepository.Update(entity);
		}
}
}
