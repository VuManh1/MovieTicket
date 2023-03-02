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
		public ShowSeatBus(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public Result AddBus(ShowSeat showSeat)
		{
			return _unitOfWork.ShowSeatRepository.Add(showSeat);
		}

		public void DeleteBus(string id)
		{
		}

		public List<ShowSeat> GetAllBus()
		{
			return _unitOfWork.ShowSeatRepository.GetAll().ToList();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.ShowSeatRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public Result UpdateBus(ShowSeat entity)
		{
			return _unitOfWork.ShowSeatRepository.Update(entity);
		}
}
}
