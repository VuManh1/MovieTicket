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
		public void AddBus(Booking Booking)
		{
			_unitOfWork.BookingRepository.Add(Booking);
		}

		public void DeleteBus(string id)
		{
			_unitOfWork.BookingRepository.Delete(id);
		}

		public void GetAllBus()
		{
			_unitOfWork.BookingRepository.GetAll();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.BookingRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
			_unitOfWork.BookingRepository.GetById(id);
		}

		public void UpdateBus(Booking entity)
		{
			_unitOfWork.BookingRepository.Update(entity);
		}
}
}
