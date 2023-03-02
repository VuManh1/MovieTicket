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
		public SeatBus(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public Result AddBus(Seat Seat)
		{
			return _unitOfWork.SeatRepository.Add(Seat);
		}

		public void DeleteBus(string id)
		{
		}

		public List<Seat> GetAllBus()
		{
			return _unitOfWork.SeatRepository.GetAll().ToList();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.SeatRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public Result UpdateBus(Seat entity)
		{
			return _unitOfWork.SeatRepository.Update(entity);
		}
}
}
