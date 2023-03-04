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

		public List<City> GetAll()
		{
			return _unitOfWork.CityRepository.GetAll().ToList();
		}

		public City? FirstOrDefault(string filter)
		{
			return _unitOfWork.CityRepository.FirstOrDefault(filter);
		}
	}
}
