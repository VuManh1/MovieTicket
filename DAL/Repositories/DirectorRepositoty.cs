using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;

namespace DAL.Repositories
{
    public class DirectorRepositoty : IRepository<Director>
    {
        private readonly IDbConnection _dbConnection;

		public DirectorRepositoty(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

        public Result Add(Director entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("AddDirector", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@Name", entity.Name);
			cmd.Parameters["@Name"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@About", entity.About);
			cmd.Parameters["@About"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(Director entity)
        {
            _dbConnection.OpenConnection();

			string query = "delete FROM Directors WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public IEnumerable<Director> Find(string filter)
        {
            throw new NotImplementedException();
        }

        public Director? FirstOrDefault(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Director> GetAll()
        {
            List<Director> Director = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM Directors;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				Director.Add(new Director
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("Name"),
					About = reader["About"].GetType() != typeof(System.DBNull) ? reader.GetString("About") : null,
				}); ;
			}
			reader.Close();

			return Director;
        }

        public Director? GetById(int id)
        {
			Director? director = null;
            _dbConnection.OpenConnection();

			string query = $"SELECT * FROM Directors WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				director = new Director
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("Name"),
					About = reader["About"].GetType() != typeof(System.DBNull) ? reader.GetString("About") : null,
				};
			}
			reader.Close();

			return director;
        }

        public Result Update(Director entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("UpdateDirector", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@name", entity.Name);
			cmd.Parameters["@name"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@About", entity.About);
			cmd.Parameters["@About"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }
}
}
