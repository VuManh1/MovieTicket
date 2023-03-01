using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class DirectorBus
    {
       private readonly IUnitOfWork _unitOfWork;
		public void AddBus(Director Director)
		{
			_unitOfWork.DirectorRepository.Add(Director);
		}

		public void DeleteBus(string id)
		{
		}

		public void GetAllBus()
		{
			_unitOfWork.DirectorRepository.GetAll();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.DirectorRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public void UpdateBus(Director entity)
		{
			_unitOfWork.DirectorRepository.Update(entity);
		}
}
}
