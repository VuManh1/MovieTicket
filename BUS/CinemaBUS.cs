using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable
namespace BUS
{
    public class CinemaBus
    {
        private readonly IUnitOfWork _unitOfWork;
		public CinemaBus(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public Result AddBus(Cinema Cinema)
		{
			return _unitOfWork.CinemaRepository.Add(Cinema);
		}

		public void DeleteBus(string id)
		{
		}

		public List<Cinema> GetAllBus()
		{
			return _unitOfWork.CinemaRepository.GetAll().ToList();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.CinemaRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public Result UpdateBus(Cinema entity)
		{
			return _unitOfWork.CinemaRepository.Update(entity);
		}
}
}
