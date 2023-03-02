using Google.Protobuf.Collections;
using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;

namespace DAL.Repositories
{
    public class MovieRepositoty : IRepository<Movie>
    {
        private readonly IDbConnection _dbConnection;

		public MovieRepositoty(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

        public Result Create(Movie entity)
        {
            _dbConnection.OpenConnection();

            MySqlCommand cmd = new("AddMovie", _dbConnection.Connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@name", entity.Name);
            cmd.Parameters["@name"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@NormalizeName", entity.NormalizeName);
            cmd.Parameters["@NormalizeName"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@description", entity.Description);
            cmd.Parameters["@description"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@length", entity.Length);
            cmd.Parameters["@length"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@ReleaseDate", entity.ReleaseDate.ToString("yyyy-MM-dd"));
            cmd.Parameters["@ReleaseDate"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@country", entity.Country);
            cmd.Parameters["@country"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@status", entity.MovieStatus.ToString());
            cmd.Parameters["@status"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@movieid", MySqlDbType.Int32);
            cmd.Parameters["@movieid"].Direction = System.Data.ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            // get id of movie
            int id = (int)cmd.Parameters["@movieid"].Value;

            return Result.OK(id);
        }

        public Result AddToMovieCast(Movie movie, List<Cast> casts)
        {
            _dbConnection.OpenConnection();

            string query = $"DELETE FROM MovieCast WHERE MovieId = {movie.Id};";

            MySqlCommand cmd = new(query, _dbConnection.Connection);
            cmd.ExecuteNonQuery();

            query = "INSERT INTO MovieCast VALUES";

            casts.ForEach(c =>
            {
                query += $" ({movie.Id}, {c.Id}),";
            });

            // remove the ','
            query = query[0..^1];
            query += ";";

            cmd.CommandText = query;
            cmd.ExecuteNonQuery();

            return Result.OK();
        }

        public Result AddToMovieDirector(Movie movie, List<Director> directors)
        {
            _dbConnection.OpenConnection();

            string query = $"DELETE FROM MovieDirector WHERE MovieId = {movie.Id};";

            MySqlCommand cmd = new(query, _dbConnection.Connection);
            cmd.ExecuteNonQuery();

            query = "INSERT INTO MovieDirector VALUES";

            directors.ForEach(d =>
            {
                query += $" ({movie.Id}, {d.Id}),";
            });

            // remove the ','
            query = query[0..^1];
            query += ";";

            cmd.CommandText = query;
            cmd.ExecuteNonQuery();

            return Result.OK();
        }

        public Result AddToMovieGenre(Movie movie, List<Genre> genres)
        {
            _dbConnection.OpenConnection();

            string query = $"DELETE FROM MovieGenre WHERE MovieId = {movie.Id};";

            MySqlCommand cmd = new(query, _dbConnection.Connection);
            cmd.ExecuteNonQuery();

            query = "INSERT INTO MovieGenre VALUES";

            genres.ForEach(d =>
            {
                query += $" ({movie.Id}, {d.Id}),";
            });

            // remove the ','
            query = query[0..^1];
            query += ";";

            cmd.CommandText = query;
            cmd.ExecuteNonQuery();

            return Result.OK();
        }

        public Result Delete(Movie entity)
        {
            _dbConnection.OpenConnection();

            string query = $"DELETE FROM Movies WHERE id = {entity.Id};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

            return Result.OK();
        }

        public IEnumerable<Movie> Find(string filter)
        {
            List<Movie> movies = new();

            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM Movies WHERE {filter};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                movies.Add(new Movie
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    NormalizeName = reader.GetString("NormalizeName"),
                    Description = reader["description"].GetType() != typeof(System.DBNull) ? reader.GetString("Description") : null,
                    Length = reader.GetInt32("length"),
                    ReleaseDate = DateOnly.FromDateTime(reader.GetDateTime("ReleaseDate")),
                    Country = reader["country"].GetType() != typeof(System.DBNull) ? reader.GetString("country") : null,
                    MovieStatus = Enum.Parse<MovieStatus>(reader.GetString("status"))
                });
            }
            reader.Close();

            return movies;
        }

        public Movie? FirstOrDefault(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Movie> GetAll()
        {
            List<Movie> movies = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM Movies;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				movies.Add(new Movie
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("name"),
					NormalizeName = reader.GetString("NormalizeName"),
					Description = reader["description"].GetType() != typeof(System.DBNull) ? reader.GetString("Description") : null,
					Length = reader.GetInt32("length"),
					ReleaseDate = DateOnly.FromDateTime(reader.GetDateTime("ReleaseDate")),
					Country = reader["country"].GetType() != typeof(System.DBNull) ? reader.GetString("country") : null,
					MovieStatus = Enum.Parse<MovieStatus>(reader.GetString("status"))
				});
			}
			reader.Close();

			return movies;
        }

        public Movie? GetById(int id)
        {
            Movie? movie = null;

            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM `movies` WHERE id = {id};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                movie = new()
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    NormalizeName = reader.GetString("NormalizeName"),
                    Description = reader["description"].GetType() != typeof(System.DBNull) ? reader.GetString("Description") : null,
                    Length = reader.GetInt32("length"),
                    ReleaseDate = DateOnly.FromDateTime(reader.GetDateTime("ReleaseDate")),
                    Country = reader["country"].GetType() != typeof(System.DBNull) ? reader.GetString("country") : null,
                    MovieStatus = Enum.Parse<MovieStatus>(reader.GetString("status"))
                };

            }
            reader.Close();

            return movie;
        }

        public Result Update(Movie entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("UpdateMovie", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@name", entity.Name);
			cmd.Parameters["@name"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@NormalizeName", entity.NormalizeName);
			cmd.Parameters["@NormalizeName"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@description", entity.Description);
			cmd.Parameters["@description"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@length", entity.Length);
			cmd.Parameters["@length"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@ReleaseDate", entity.ReleaseDate.ToString("yyyy-MM-dd"));
			cmd.Parameters["@ReleaseDate"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@country", entity.Country);
			cmd.Parameters["@country"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@status", entity.MovieStatus.ToString());
            cmd.Parameters["@status"].Direction = System.Data.ParameterDirection.Input;

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public List<Genre> GetGenres(Movie movie)
        {
            List<Genre> genres = new();

            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM `MovieGenreView` WHERE MovieId = {movie.Id};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                genres.Add(new Genre
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Description = reader["description"].GetType() != typeof(System.DBNull) ? reader.GetString("description") : null,
                });
            }
            reader.Close();

            return genres;
        }

        public List<Cast> GetCasts(Movie movie)
        {
            List<Cast> casts = new();

            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM `MovieCastView` WHERE MovieId = {movie.Id};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                casts.Add(new Cast
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    About = reader["about"].GetType() != typeof(System.DBNull) ? reader.GetString("about") : null,
                });
            }
            reader.Close();

            return casts;
        }

        public List<Director> GetDirectors(Movie movie)
        {
            List<Director> directors = new();

            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM `MovieDirectorView` WHERE MovieId = {movie.Id};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                directors.Add(new Director
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    About = reader["about"].GetType() != typeof(System.DBNull) ? reader.GetString("about") : null,
                });
            }
            reader.Close();

            return directors;
        }
    }
}
