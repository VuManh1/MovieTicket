using SharedLibrary;
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

		public Result Create(Genre Genre)
		{
			return _unitOfWork.GenreRepository.Create(Genre);
		}

		public void Delete(string id)
		{
		}

		public List<Genre> GetAll()
		{
			return _unitOfWork.GenreRepository.GetAll().ToList();
		}

		public void FirstOrDefault(string filter)
		{
			_unitOfWork.GenreRepository.FirstOrDefault(filter);
		}

		public void GetById(string id)
		{
		}

		public Result UpdateBus(Genre entity)
		{
			return _unitOfWork.GenreRepository.Update(entity);
		}
}
}
