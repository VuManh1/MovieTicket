using SharedLibrary.DTO;
using DAL.UnitOfWork;
#pragma warning disable

namespace BUS
{
    public class GenreBus
    {
       private readonly IUnitOfWork _unitOfWork;
		public void AddBus(Genre Genre)
		{
			_unitOfWork.GenreRepository.Add(Genre);
		}

		public void DeleteBus(string id)
		{
			_unitOfWork.GenreRepository.Delete(id);
		}

		public void GetAllBus()
		{
			_unitOfWork.GenreRepository.GetAll();
		}

		public void FirstOrDefaultBus(string filter)
		{
			_unitOfWork.GenreRepository.FirstOrDefault(filter);
		}

		public void GetByIdBus(string id)
		{
			_unitOfWork.GenreRepository.GetById(id);
		}

		public void UpdateBus(Genre entity)
		{
			_unitOfWork.GenreRepository.Update(entity);
		}
}
}
