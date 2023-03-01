using SharedLibrary.DTO;
using DAL.UnitOfWork;
using SharedLibrary.Models;
using SharedLibrary.Helpers;
using SharedLibrary;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Linq;

namespace BUS
{
    public class MovieBus
    {
        private readonly IUnitOfWork _unitOfWork;

		public MovieBus(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Add(MovieModel model)
		{
			Movie movie = new()
			{
				Name = model.Name,
				NormalizeName = model.Name.NormalizeString(),
				Description = model.Description,
				Length = model.Length,
				MovieStatus = model.MovieStatus,
				Country = model.Country,
				ReleaseDate = model.ReleaseDate,
			};

			// add casts
			if (model.Casts != null)
			{
				string[] idList = Regex.Split(model.Casts, ", |,");

				foreach (string idItem in idList)
				{
					if (!int.TryParse(idItem, out int id) || id == 0) continue;

                    Cast? cast = _unitOfWork.CastRepository.GetById(id);

					if (cast != null)
					{
						movie.Casts ??= new List<Cast>();

                        if (!movie.Casts.Contains(cast))
                            movie.Casts.Add(cast);
					}
				}
			}

            // add directors
            if (model.Directors != null)
            {
                string[] idList = Regex.Split(model.Directors, ", |,");

                foreach (string idItem in idList)
                {
                    if (!int.TryParse(idItem, out int id) || id == 0) continue;

                    Director? director = _unitOfWork.DirectorRepository.GetById(id);

                    if (director != null)
                    {
                        movie.Directors ??= new List<Director>();
                        
						if(!movie.Directors.Contains(director)) 
							movie.Directors.Add(director);
                    }
                }
            }

			_unitOfWork.BeginTransaction();
			try
			{
                Result result = _unitOfWork.MovieRepository.Add(movie);

				_unitOfWork.CommitTransaction();
            }
            catch
			{
				_unitOfWork.RollBack();
				return Result.Error();
			}

			return Result.OK();
        }

		public void Delete(int id)
		{
		}

		public List<Movie> GetAll()
		{
			return _unitOfWork.MovieRepository.GetAll().ToList();
		}

		public void FirstOrDefault(string filter)
		{
			_unitOfWork.MovieRepository.FirstOrDefault(filter);
		}

		public void GetById(int id)
		{
			_unitOfWork.MovieRepository.GetById(id);
		}

		public void Update(Movie entity)
		{
			_unitOfWork.MovieRepository.Update(entity);
		}
}
}
