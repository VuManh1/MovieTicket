using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
using SharedLibrary.Helpers;

namespace BUS
{
    public class CinemaBUS
    {
        private readonly IUnitOfWork _unitOfWork;

		public CinemaBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Create(Cinema cinema)
		{
            cinema.Name = cinema.Name.NormalizeString();

            try
            {
                return _unitOfWork.CinemaRepository.Create(cinema);
            }
            catch
            {
                return Result.NetworkError();
            }
        }

		public Result Delete(Cinema cinema)
		{
			try
			{
				return _unitOfWork.CinemaRepository.Delete(cinema);
			}
			catch
			{
				return Result.NetworkError();
			}
		}

		public List<Cinema> GetAll()
		{
            try
            {
                return _unitOfWork.CinemaRepository.GetAll().ToList();
            }
            catch
            {
                return new List<Cinema>();
            }
        }

        public List<Cinema> Find(string filter)
        {
            try
            {
                return _unitOfWork.CinemaRepository.Find(filter).ToList();
            }
            catch
            {
				return new List<Cinema>();
            }
        }

        public Cinema? FirstOrDefault(string filter)
		{
			return _unitOfWork.CinemaRepository.FirstOrDefault(filter);
		}

		public Cinema? GetById(int id)
		{
            try
            {
                return _unitOfWork.CinemaRepository.GetById(id);
            }
            catch
            {
                return null;
            }
        }

		public Result Update(Cinema entity)
		{
			return _unitOfWork.CinemaRepository.Update(entity);
		}
}
}
