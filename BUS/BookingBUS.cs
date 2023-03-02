using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class BookingBus
    {
        private readonly IUnitOfWork _unitOfWork;
		public Result AddBus(Booking Booking)
		{
			return _unitOfWork.BookingRepository.Add(Booking);
		}

		public void DeleteBus(string id)
		{
		}

		public List<Booking> GetAllBus()
		{
			return _unitOfWork.BookingRepository.GetAll().ToList();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.BookingRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public Result UpdateBus(Booking entity)
		{
			return _unitOfWork.BookingRepository.Update(entity);
		}
}
}
