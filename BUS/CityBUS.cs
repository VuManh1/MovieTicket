using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;

namespace BUS
{
    public class CityBUS
    {
        private readonly IUnitOfWork _unitOfWork;

		public CityBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Create(City City)
		{
			return _unitOfWork.CityRepository.Create(City);
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
