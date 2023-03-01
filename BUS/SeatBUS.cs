using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class SeatBus
    {
        private readonly IUnitOfWork _unitOfWork;
		public void AddBus(Seat Seat)
		{
			_unitOfWork.SeatRepository.Add(Seat);
		}

		public void DeleteBus(string id)
		{
		}

		public void GetAllBus()
		{
			_unitOfWork.SeatRepository.GetAll();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.SeatRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public void UpdateBus(Seat entity)
		{
			_unitOfWork.SeatRepository.Update(entity);
		}
}
}
