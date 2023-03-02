using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class ShowBus
    {
        private readonly IUnitOfWork _unitOfWork;
		public ShowBus(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public Result AddBus(Show Show)
		{
			return _unitOfWork.ShowRepository.Add(Show);
		}

		public void DeleteBus(string id)
		{
		}

		public List<Show> GetAllBus()
		{
			return _unitOfWork.ShowRepository.GetAll().ToList();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.ShowRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public Result UpdateBus(Show entity)
		{
			return _unitOfWork.ShowRepository.Update(entity);
		}
}
}
