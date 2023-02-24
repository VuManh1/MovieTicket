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
		public void AddBus(City City)
		{
			_unitOfWork.CityRepository.Add(City);
		}

		public void DeleteBus(string id)
		{
			_unitOfWork.CityRepository.Delete(id);
		}

		public void GetAllBus()
		{
			_unitOfWork.CityRepository.GetAll();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.CityRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
			_unitOfWork.CityRepository.GetById(id);
		}

		public void UpdateBus(City entity)
		{
			_unitOfWork.CityRepository.Update(entity);
		}
}
}
