using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class MovieBus
    {
        private readonly IUnitOfWork _unitOfWork;

		public MovieBus(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public void AddBus(Movie Movie)
		{
			_unitOfWork.MovieRepository.Add(Movie);
		}

		public void DeleteBus(string id)
		{
			_unitOfWork.MovieRepository.Delete(id);
		}

		public List<Movie> GetAllBus()
		{
			return _unitOfWork.MovieRepository.GetAll().ToList();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.MovieRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
			_unitOfWork.MovieRepository.GetById(id);
		}

		public void UpdateBus(Movie entity)
		{
			_unitOfWork.MovieRepository.Update(entity);
		}
}
}
