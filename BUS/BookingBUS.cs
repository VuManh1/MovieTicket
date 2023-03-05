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

        public Result Create(Booking booking)
		{
			_unitOfWork.BeginTransaction();
			try 
			{
				Result result = _unitOfWork.BookingRepository.Create(booking);

				_unitOfWork.BookingRepository.UpdateShowSeat(booking);

				_unitOfWork.CommitTransaction();
			}
			catch
			{
				_unitOfWork.RollBack();
				return Result.Error();
			}

            return Result.OK();
        }

		public List<ShowSeat> GetShowSeats(Booking booking)
		{
			try
			{
				return _unitOfWork.BookingRepository.GetShowSeats(booking).ToList();
            }
			catch
			{
				return new List<ShowSeat>();
			}
		}

        public Result Delete(Booking booking)
		{
			try
			{
				return _unitOfWork.BookingRepository.Delete(booking);
			}
			catch
			{
				return Result.Error();
			}
        }

        public List<Booking> GetByUserId(int id)
        {
            try
            {
                return _unitOfWork.BookingRepository.Find($"UserId = {id}").ToList();
            }
            catch
            {
                return new List<Booking>();
            }
        }

        public Booking? GetById(int id)
		{
			try
			{
				return _unitOfWork.BookingRepository.GetById(id);
			}
			catch
			{
				return null;
			}
		}
	}
}
