using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;

namespace DAL.Repositories
{
    public class GenreRepositoty : IRepository<Genre>
    {
        private readonly IDbConnection _dbConnection;

		public GenreRepositoty(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

        public Result Add(Genre entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("AddGenre", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Name", entity.Name);
			cmd.Parameters["@Name"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Description", entity.Description);
			cmd.Parameters["@Description"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(string id)
        {
            _dbConnection.OpenConnection();

			string query = "delete FROM Genres WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Genre? FirstOrDefault(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Genre> GetAll()
        {
            List<Genre> Genre = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM Genres;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				Genre.Add(new Genre
				{
					Id = reader.GetString("id"),
					Name = reader.GetString("Name"),
					Description = reader["Description"].GetType() != typeof(System.DBNull) ? reader.GetString("Description") : null,
				}); ;
			}
			reader.Close();

			return Genre;
        }

        public Genre? GetById(string id)
        {
			Genre? genre = null;
            _dbConnection.OpenConnection();

			string query = "SELECT * FROM Genres WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				genre = new Genre
				{
					Id = reader.GetString("id"),
					Name = reader.GetString("Name"),
					Description = reader["Description"].GetType() != typeof(System.DBNull) ? reader.GetString("Description") : null,
				} ;
			}
			reader.Close();

			return genre;
        }

        public Result Update(Genre entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("UpdateGenre", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@name", entity.Name);
			cmd.Parameters["@name"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Description", entity.Description);
			cmd.Parameters["@Description"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }
}
}
