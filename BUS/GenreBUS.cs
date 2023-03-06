using SharedLibrary.DTO;
using DAL.UnitOfWork;

namespace BUS
{
    public class GenreBUS
    {
       private readonly IUnitOfWork _unitOfWork;

	   public GenreBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public List<Genre> GetAll()
		{
			try
			{
				return _unitOfWork.GenreRepository.GetAll().ToList();
			}
			catch
			{
				return new List<Genre>();
			}
		}

		public List<Movie> GetMovies(Genre genre)
		{
            try
            {
                return _unitOfWork.GenreRepository.GetMovies(genre);
            }
            catch
            {
                return new List<Movie>();
            }
        }
	}
}
