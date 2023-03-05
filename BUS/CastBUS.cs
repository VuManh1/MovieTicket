using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
using SharedLibrary.Helpers;
using SharedLibrary.Models;

namespace BUS
{
    public class CastBUS
    {
       private readonly IUnitOfWork _unitOfWork;

	   public CastBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Create(Cast cast)
		{
			cast.Name = cast.Name.NormalizeString();

			try
			{
				return _unitOfWork.CastRepository.Create(cast);
			}
			catch
			{
				return Result.NetworkError();
			}
		}

		public Result Delete(Cast cast)
		{
			try
			{
				return _unitOfWork.CastRepository.Delete(cast);
			}
			catch
			{
				return Result.NetworkError();
			}
		}

        public List<Cast> Find(string filter)
        {
			try
			{
				return _unitOfWork.CastRepository.Find($"name like '%{filter}%'").ToList();
			}
			catch
			{
				return new List<Cast>();
			}
        }

        public List<Cast> GetAll()
		{
            try
            {
                return _unitOfWork.CastRepository.GetAll().ToList();
            }
            catch
            {
                return new List<Cast>();
            }
        }

		public Cast? GetById(int id)
		{
            return _unitOfWork.CastRepository.GetById(id);
        }

		public Result Update(Cast entity)
		{
            entity.Name = entity.Name.NormalizeString();

            try
            {
                return _unitOfWork.CastRepository.Update(entity);
            }
            catch
            {
                return Result.NetworkError();
            }
        }
}
}
