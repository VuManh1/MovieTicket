using SharedLibrary.DTO;
using DAL.UnitOfWork;
using SharedLibrary;
using System.Text.RegularExpressions;
using SharedLibrary.Helpers;

namespace BUS
{
    public class MovieBUS
    {
        private readonly IUnitOfWork _unitOfWork;

		public MovieBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Create(Movie movie)
		{
            movie.Name = movie.Name.NormalizeString();
            movie.NormalizeName = movie.Name.RemoveMarks();

            List<Genre>? genres = null;
            // add genres
            if (movie.GenreString != null)
            {
                string[] genreList = Regex.Split(movie.GenreString, ", |,");

                foreach (string genreItem in genreList)
                {
                    Genre? genre = _unitOfWork.GenreRepository.FirstOrDefault($"name = '{genreItem}'");

                    if (genre != null)
                    {
                        genres ??= new List<Genre>();

                        if (!genres.Contains(genre))
                            genres.Add(genre);
                    }
                }
            }

            List<Cast>? casts = null;
			// add casts
			if (movie.CastIdString != null)
			{
				string[] idList = Regex.Split(movie.CastIdString, ", |,");

				foreach (string idItem in idList)
				{
					if (!int.TryParse(idItem, out int id) || id == 0) continue;

                    Cast? cast = _unitOfWork.CastRepository.GetById(id);

					if (cast != null)
					{
                        casts ??= new List<Cast>();

                        if (!casts.Contains(cast))
                            casts.Add(cast);
					}
				}
			}
            
			List<Director>? directors = null;
            // add directors
            if (movie.DirectorIdString != null)
            {
                string[] idList = Regex.Split(movie.DirectorIdString, ", |,");

                foreach (string idItem in idList)
                {
                    if (!int.TryParse(idItem, out int id) || id == 0) continue;

                    Director? director = _unitOfWork.DirectorRepository.GetById(id);

                    if (director != null)
                    {
                        directors ??= new List<Director>();
                        
						if(!directors.Contains(director))
                            directors.Add(director);
                    }
                }
            }

			_unitOfWork.BeginTransaction();
			try
			{
                Result result = _unitOfWork.MovieRepository.Create(movie);

				if (genres != null) _unitOfWork.MovieRepository.AddToMovieGenre(movie, genres);
				if (casts != null) _unitOfWork.MovieRepository.AddToMovieCast(movie, casts);
                if (directors != null) _unitOfWork.MovieRepository.AddToMovieDirector(movie, directors);

                _unitOfWork.CommitTransaction();
            }
            catch(Exception e)
			{
				_unitOfWork.RollBack();
				return Result.Error(e.Message);
			}

			return Result.OK();
        }

        public List<Movie> Find(string filter)
        {
            try
            {
                return _unitOfWork.MovieRepository.Find($"NormalizeName like '%{filter}%'").ToList();
            }
            catch
            {
                return new List<Movie>();
            }
        }

        public List<Movie> GetByStatus(MovieStatus status)
        {
            try
            {
                return _unitOfWork.MovieRepository.Find($"status = '{status}'").ToList();
            }
            catch
            {
                return new List<Movie>();
            }
        }

        public Result Delete(Movie movie)
		{
			try
			{
				return _unitOfWork.MovieRepository.Delete(movie);

			}
			catch
			{
				return Result.NetworkError();
			}
		}

		public List<Movie> GetAll()
		{
			return _unitOfWork.MovieRepository.GetAll().ToList();
		}

		public Movie? GetById(int id)
		{
			return _unitOfWork.MovieRepository.GetById(id);
		}

		public Result Update(Movie movie)
		{
            movie.Name = movie.Name.NormalizeString();
            movie.NormalizeName = movie.Name.RemoveMarks();

            List<Genre>? genres = null;
            // add genres
            if (movie.GenreString != null)
            {
                string[] genreList = Regex.Split(movie.GenreString, ", |,");

                foreach (string genreItem in genreList)
                {
                    Genre? genre = _unitOfWork.GenreRepository.FirstOrDefault($"name = '{genreItem}'");

                    if (genre != null)
                    {
                        genres ??= new List<Genre>();

                        if (!genres.Contains(genre))
                            genres.Add(genre);
                    }
                }
            }

            List<Cast>? casts = null;
            // add casts
            if (movie.CastIdString != null)
            {
                string[] idList = Regex.Split(movie.CastIdString, ", |,");

                foreach (string idItem in idList)
                {
                    if (!int.TryParse(idItem, out int id) || id == 0) continue;

                    Cast? cast = _unitOfWork.CastRepository.GetById(id);

                    if (cast != null)
                    {
                        casts ??= new List<Cast>();

                        if (!casts.Contains(cast))
                            casts.Add(cast);
                    }
                }
            }

            List<Director>? directors = null;
            // add directors
            if (movie.DirectorIdString != null)
            {
                string[] idList = Regex.Split(movie.DirectorIdString, ", |,");

                foreach (string idItem in idList)
                {
                    if (!int.TryParse(idItem, out int id) || id == 0) continue;

                    Director? director = _unitOfWork.DirectorRepository.GetById(id);

                    if (director != null)
                    {
                        directors ??= new List<Director>();

                        if (!directors.Contains(director))
                            directors.Add(director);
                    }
                }
            }

            _unitOfWork.BeginTransaction();
            try
            {
                _unitOfWork.MovieRepository.Update(movie);

                if (genres != null) _unitOfWork.MovieRepository.AddToMovieGenre(movie, genres);
                if (casts != null) _unitOfWork.MovieRepository.AddToMovieCast(movie, casts);
                if (directors != null) _unitOfWork.MovieRepository.AddToMovieDirector(movie, directors);

                _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                _unitOfWork.RollBack();
                return Result.Error(e.Message);
            }

            return Result.OK();
        }

        public List<Genre> GetGenres(Movie movie)
        {
            return _unitOfWork.MovieRepository.GetGenres(movie);
        }

        public List<Cast> GetCasts(Movie movie)
		{
			return _unitOfWork.MovieRepository.GetCasts(movie);
		
        }

        public List<Director> GetDirectors(Movie movie)
        {
            return _unitOfWork.MovieRepository.GetDirectors(movie);
        }

    }
}
