using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class ShowSeatBus
    {
        private readonly IUnitOfWork _unitOfWork;
		public void AddBus(ShowSeat showSeat)
		{
			_unitOfWork.ShowSeatRepository.Add(showSeat);
		}

		public void DeleteBus(string id)
		{
		}

		public void GetAllBus()
		{
			_unitOfWork.ShowSeatRepository.GetAll();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.ShowSeatRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public void UpdateBus(ShowSeat entity)
		{
			_unitOfWork.ShowSeatRepository.Update(entity);
		}
}
}
