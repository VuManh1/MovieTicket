using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class CastBus
    {
       private readonly IUnitOfWork _unitOfWork;
	   public CastBus(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public Result AddBus(Cast cast)
		{
			return _unitOfWork.CastRepository.Add(cast);
		}

		public void DeleteBus(string id)
		{
		}

		public List<Cast> GetAllBus()
		{
			return _unitOfWork.CastRepository.GetAll().ToList();
		}

		public Cast? FirstOrDefaultBus(string filter)
		{
			return _unitOfWork.CastRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public Result UpdateBus(Cast entity)
		{
			return _unitOfWork.CastRepository.Update(entity);
		}
}
}
