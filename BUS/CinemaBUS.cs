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
		public void AddBus(Cinema Cinema)
		{
			_unitOfWork.CinemaRepository.Add(Cinema);
		}

		public void DeleteBus(string id)
		{
			_unitOfWork.CinemaRepository.Delete(id);
		}

		public void GetAllBus()
		{
			_unitOfWork.CinemaRepository.GetAll();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.CinemaRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
			_unitOfWork.CinemaRepository.GetById(id);
		}

		public void UpdateBus(Cinema entity)
		{
			_unitOfWork.CinemaRepository.Update(entity);
		}
}
}
