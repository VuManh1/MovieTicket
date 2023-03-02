using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;

namespace BUS
{
    public class HallBUS
    {
        private readonly IUnitOfWork _unitOfWork;

		public HallBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Create(Hall Hall)
		{
			return _unitOfWork.HallRepository.Create(Hall);
		}

		public void Delete(string id)
		{
		}

		public List<Hall> GetAll()
		{
			return _unitOfWork.HallRepository.GetAll().ToList();
		}

		public void FirstOrDefault(string filter)
		{
			_unitOfWork.HallRepository.FirstOrDefault(filter);
		}

		public void GetById(string id)
		{
		}

		public Result Update(Hall entity)
		{
			return _unitOfWork.HallRepository.Update(entity);
		}
}
}
