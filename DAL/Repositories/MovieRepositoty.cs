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

        public Result Add(Movie entity)
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
            cmd.Parameters.AddWithValue("@ReleaseDate", entity.ReleaseDate?.ToString("yyyy-MM-dd"));
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

            // insert to MovieCast table
            if (entity.Casts != null)
            {
                string query = "INSERT INTO MovieCast VALUES";

                entity.Casts.ForEach(c =>
                {
                    query += $" ({id}, {c.Id}),";
                });

                // remove the ','
                query = query[0..^1];
                query += ";";

                cmd.Parameters.Clear();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = query;

                cmd.ExecuteNonQuery();
            }

            // insert to MovieDirector table
            if (entity.Directors != null)
            {
                string query = "INSERT INTO MovieDirector VALUES";

                entity.Directors.ForEach(c =>
                {
                    query += $" ({id}, {c.Id}),";
                });

                // remove the ','
                query = query[0..^1];
                query += ";";

                cmd.Parameters.Clear();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = query;

                cmd.ExecuteNonQuery();
            }

            return Result.OK();
        }

        public Result Delete(Movie entity)
        {
            _dbConnection.OpenConnection();

			return Result.OK();
        }

        public IEnumerable<Movie> Find(string filter)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            
			cmd.Parameters.AddWithValue("@ReleaseDate", entity.ReleaseDate?.ToString("yyyy-MM-dd"));
			cmd.Parameters["@ReleaseDate"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@country", entity.Country);
			cmd.Parameters["@country"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }
}
}
