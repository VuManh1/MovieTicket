using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class DirectorBus
    {
       private readonly IUnitOfWork _unitOfWork;
	   public DirectorBus(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public Result AddBus(Director Director)
		{
			return _unitOfWork.DirectorRepository.Add(Director);
		}

		public void DeleteBus(string id)
		{
		}

		public List<Director> GetAllBus()
		{
			return _unitOfWork.DirectorRepository.GetAll().ToList();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.DirectorRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public Result UpdateBus(Director entity)
		{
			return _unitOfWork.DirectorRepository.Update(entity);
		}
}
}
