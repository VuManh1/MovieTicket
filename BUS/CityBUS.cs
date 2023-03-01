using SharedLibrary.DTO;
using DAL.UnitOfWork;

namespace BUS
{
    public class CityBus
    {
        private readonly IUnitOfWork _unitOfWork;

		public CityBus(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public void Add(City City)
		{
			_unitOfWork.CityRepository.Add(City);
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

		public void Update(City entity)
		{
			_unitOfWork.CityRepository.Update(entity);
		}
}
}
