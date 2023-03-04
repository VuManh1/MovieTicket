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
			return _unitOfWork.GenreRepository.GetAll().ToList();
		}
	}
}
