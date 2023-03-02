using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class HallBus
    {
        private readonly IUnitOfWork _unitOfWork;
		public HallBus(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public Result AddBus(Hall Hall)
		{
			return _unitOfWork.HallRepository.Add(Hall);
		}

		public void DeleteBus(string id)
		{
		}

		public List<Hall> GetAllBus()
		{
			return _unitOfWork.HallRepository.GetAll().ToList();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.HallRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public Result UpdateBus(Hall entity)
		{
			return _unitOfWork.HallRepository.Update(entity);
		}
}
}
