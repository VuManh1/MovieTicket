using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;

namespace BUS
{
    public class SeatBUS
    {
        private readonly IUnitOfWork _unitOfWork;

		public SeatBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Create(Seat Seat)
		{
			return _unitOfWork.SeatRepository.Create(Seat);
		}

		public void Delete(string id)
		{
		}

		public List<Seat> GetAll()
		{
			return _unitOfWork.SeatRepository.GetAll().ToList();
		}

		public void FirstOrDefault(string filter)
		{
			_unitOfWork.SeatRepository.FirstOrDefault(filter);
		}

		public void GetById(string id)
		{
		}

		public Result Update(Seat entity)
		{
			return _unitOfWork.SeatRepository.Update(entity);
		}
}
}
