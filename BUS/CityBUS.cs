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
			try
			{
				return _unitOfWork.CityRepository.GetAll().ToList();
			}
			catch
			{
				return new List<City>();
			}
		}

		public City? GetByName(string name)
		{
			try
			{
				return _unitOfWork.CityRepository.FirstOrDefault($"name = '{name}'");
			}
			catch
			{
				return null;
			}
		}
	}
}
