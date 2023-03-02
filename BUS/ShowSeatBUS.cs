using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;

namespace BUS
{
    public class ShowSeatBUS
    {
        private readonly IUnitOfWork _unitOfWork;

		public ShowSeatBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Create(ShowSeat showSeat)
		{
			return _unitOfWork.ShowSeatRepository.Create(showSeat);
		}

		public void Delete(string id)
		{
		}

		public List<ShowSeat> GetAll()
		{
			return _unitOfWork.ShowSeatRepository.GetAll().ToList();
		}

		public void FirstOrDefault(string filter)
		{
			_unitOfWork.ShowSeatRepository.FirstOrDefault(filter);
		}

		public void GetById(string id)
		{
		}

		public Result Update(ShowSeat entity)
		{
			return _unitOfWork.ShowSeatRepository.Update(entity);
		}
}
}
