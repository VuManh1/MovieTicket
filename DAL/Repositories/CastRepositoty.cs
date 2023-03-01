using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;

namespace DAL.Repositories
{
    public class CastRepositoty : IRepository<Cast>
    {
        private readonly IDbConnection _dbConnection;

		public CastRepositoty(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

        public Result Add(Cast entity)
        {
            _dbConnection.OpenConnection();

            string query = $"INSERT INTO Casts(name, about) VALUES('{entity.Name}', '{entity.About}');";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(Cast entity)
        {
            _dbConnection.OpenConnection();

			string query = "delete FROM Casts WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public IEnumerable<Cast> Find(string filter)
        {
            throw new NotImplementedException();
        }

        public Cast? FirstOrDefault(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cast> GetAll()
        {
            List<Cast> cast = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM Casts;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				cast.Add(new Cast
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("name"),
					About = reader["About"].GetType() != typeof(System.DBNull) ? reader.GetString("About") : null,
				}); ;
			}
			reader.Close();

			return cast;
        }

        public Cast? GetById(int id)
        {
			Cast? cast = null;
            _dbConnection.OpenConnection();

			string query = $"SELECT * FROM Casts WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				cast = new Cast
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("name"),
					About = reader["About"].GetType() != typeof(System.DBNull) ? reader.GetString("About") : null,
				} ;
			}
			reader.Close();

			return cast;
        }

        public Result Update(Cast entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("UpdateCast", _dbConnection.Connection)
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
