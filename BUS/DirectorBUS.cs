using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;

namespace BUS
{
    public class DirectorBUS
    {
       private readonly IUnitOfWork _unitOfWork;

	   public DirectorBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Create(Director director)
		{
            try
            {
                return _unitOfWork.DirectorRepository.Create(director);
            }
            catch
            {
                return Result.NetworkError();
            }
        }

		public Result Delete(Director director)
		{
            try
            {
                return _unitOfWork.DirectorRepository.Delete(director);
            }
            catch
            {
                return Result.NetworkError();
            }
        }

        public List<Director> Find(string filter)
        {
            try
            {
                return _unitOfWork.DirectorRepository.Find(filter).ToList();
            }
            catch
            {
                return new List<Director>();
            }
        }

        public List<Director> GetAll()
		{
            try
            {
                return _unitOfWork.DirectorRepository.GetAll().ToList();
            }
            catch
            {
                return new List<Director>();
            }
        }

		public Director? GetById(int id)
		{
            return _unitOfWork.DirectorRepository.GetById(id);
        }

        public Result Update(Director entity)
		{
            try
            {
                return _unitOfWork.DirectorRepository.Update(entity);
            }
            catch
            {
                return Result.NetworkError();
            }
        }
}
}
