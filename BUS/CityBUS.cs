using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class CityBus
    {
        private readonly IUnitOfWork _unitOfWork;

		public CityBus(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Add(City City)
		{
			return _unitOfWork.CityRepository.Add(City);
		}

		public void Delete(string id)
		{
		}

		public List<City> GetAll()
		{
			return _unitOfWork.CityRepository.GetAll().ToList();
		}

		public City? FirstOrDefault(string filter)
		{
			return _unitOfWork.CityRepository.FirstOrDefault(filter);
		}

		public void GetById(string id)
		{
		}

		public Result Update(City entity)
		{
			return _unitOfWork.CityRepository.Update(entity);
		}
}
}
