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

        public Result Create(Genre entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("AddGenre", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@name", entity.Name);
			cmd.Parameters["@Name"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@description", entity.Description);
			cmd.Parameters["@description"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(Genre entity)
        {
            _dbConnection.OpenConnection();

			string query = $"DELETE FROM Genres WHERE id = {entity.Id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public IEnumerable<Genre> Find(string filter)
        {
            throw new NotImplementedException();
        }

        public Genre? FirstOrDefault(string filter)
        {
            Genre? genre = null;
            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM Genres WHERE {filter};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

			if (reader.HasRows)
            {
				reader.Read();

                genre = new()
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Description = reader["description"].GetType() != typeof(System.DBNull) ? reader.GetString("description") : null,
                };
            }
            reader.Close();

            return genre;
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
					Id = reader.GetInt32("id"),
					Name = reader.GetString("name"),
					Description = reader["description"].GetType() != typeof(System.DBNull) ? reader.GetString("description") : null,
				}); ;
			}
			reader.Close();

			return Genre;
        }

        public Genre? GetById(int id)
        {
			Genre? genre = null;
            _dbConnection.OpenConnection();

			string query = $"SELECT * FROM Genres WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                genre = new()
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("name"),
					Description = reader["description"].GetType() != typeof(System.DBNull) ? reader.GetString("description") : null,
				};
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

			cmd.Parameters.AddWithValue("@description", entity.Description);
			cmd.Parameters["@description"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }
}
}
