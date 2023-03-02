using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;

namespace BUS
{
    public class DirectorBUS
    {
       private readonly IUnitOfWork _unitOfWork;

	   public DirectorBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Create(Director Director)
		{
			return _unitOfWork.DirectorRepository.Create(Director);
		}

		public void Delete(string id)
		{
		}

		public List<Director> GetAll()
		{
			return _unitOfWork.DirectorRepository.GetAll().ToList();
		}

		public void FirstOrDefault(string filter)
		{
			_unitOfWork.DirectorRepository.FirstOrDefault(filter);
		}

		public void GetById(string id)
		{
		}

		public Result Update(Director entity)
		{
			return _unitOfWork.DirectorRepository.Update(entity);
		}
}
}
