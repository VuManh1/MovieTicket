using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;

namespace BUS
{
    public class BookingBUS
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingBUS(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result Create(Booking Booking)
		{
			return _unitOfWork.BookingRepository.Create(Booking);
		}

		public void Delete(string id)
		{
		}

		public List<Booking> GetAll()
		{
			return _unitOfWork.BookingRepository.GetAll().ToList();
		}

		public void FirstOrDefault(string filter)
		{
			_unitOfWork.BookingRepository.FirstOrDefault(filter);
		}

		public void GetById(string id)
		{
		}

		public Result Update(Booking entity)
		{
			return _unitOfWork.BookingRepository.Update(entity);
		}
}
}
