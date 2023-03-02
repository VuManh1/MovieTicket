using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;

namespace BUS
{
    public class ShowBUS
    {
        private readonly IUnitOfWork _unitOfWork;

		public ShowBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Create(Show Show)
		{
			return _unitOfWork.ShowRepository.Create(Show);
		}

		public void Delete(string id)
		{
		}

		public List<Show> GetAll()
		{
			return _unitOfWork.ShowRepository.GetAll().ToList();
		}

		public void FirstOrDefault(string filter)
		{
			_unitOfWork.ShowRepository.FirstOrDefault(filter);
		}

		public void GetById(string id)
		{
		}

		public Result Update(Show entity)
		{
			return _unitOfWork.ShowRepository.Update(entity);
		}
}
}
