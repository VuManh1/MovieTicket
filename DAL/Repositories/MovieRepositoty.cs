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

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@name", entity.Name);
			cmd.Parameters["@name"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@NormalizeName", entity.NormalizeName);
			cmd.Parameters["@NormalizeName"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Description", entity.Description);
			cmd.Parameters["@Description"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Length", entity.Length);
			cmd.Parameters["@Length"].Direction = System.Data.ParameterDirection.Input;
            
			cmd.Parameters.AddWithValue("@ReleaseDate", entity.ReleaseDate?.ToString("yyyy-MM-dd"));
			cmd.Parameters["@ReleaseDate"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Poster", entity);
			cmd.Parameters["@Poster"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Country", entity.Country);
			cmd.Parameters["@Country"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(string id)
        {
            _dbConnection.OpenConnection();

			id = "delete FROM Movies;";
			MySqlCommand cmd = new(id, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
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
					Id = reader.GetString("id"),
					Name = reader.GetString("name"),
					NormalizeName = reader.GetString("NormalizeName"),
					Description = reader["Description"].GetType() != typeof(System.DBNull) ? reader.GetString("Description") : null,
					Length = reader.GetInt32("Length"),
					ReleaseDate = reader["ReleaseDate"].GetType() != typeof(System.DBNull) ? DateOnly.FromDateTime(reader.GetDateTime("ReleaseDate")) : null,
                    Poster = reader.GetString("Poster"),
					Country = reader.GetString("Country")
				}); ;
			}
			reader.Close();

			return movies;
        }

        public Movie? GetById(string id)
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

			cmd.Parameters.AddWithValue("@Description", entity.Description);
			cmd.Parameters["@Description"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Length", entity.Length);
			cmd.Parameters["@Length"].Direction = System.Data.ParameterDirection.Input;
            
			cmd.Parameters.AddWithValue("@ReleaseDate", entity.ReleaseDate?.ToString("yyyy-MM-dd"));
			cmd.Parameters["@ReleaseDate"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Poster", entity);
			cmd.Parameters["@Poster"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Country", entity.Country);
			cmd.Parameters["@Country"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }
}
}
