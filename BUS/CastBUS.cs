using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;

namespace BUS
{
    public class CastBUS
    {
       private readonly IUnitOfWork _unitOfWork;

	   public CastBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Create(Cast cast)
		{
			return _unitOfWork.CastRepository.Create(cast);
		}

		public void Delete(string id)
		{
		}

		public List<Cast> GetAll()
		{
			return _unitOfWork.CastRepository.GetAll().ToList();
		}

		public Cast? FirstOrDefault(string filter)
		{
			return _unitOfWork.CastRepository.FirstOrDefault(filter);
		}

		public void GetById(string id)
		{
		}

		public Result Update(Cast entity)
		{
			return _unitOfWork.CastRepository.Update(entity);
		}
}
}
