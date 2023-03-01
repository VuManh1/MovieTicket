using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class GenreBus
    {
       private readonly IUnitOfWork _unitOfWork;
	   public GenreBus(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public Result AddBus(Genre Genre)
		{
			return _unitOfWork.GenreRepository.Add(Genre);
		}

		public void DeleteBus(string id)
		{
		}

		public List<Genre> GetAllBus()
		{
			return _unitOfWork.GenreRepository.GetAll().ToList();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.GenreRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
		}

		public Result UpdateBus(Genre entity)
		{
			return _unitOfWork.GenreRepository.Update(entity);
		}
}
}
