using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;

namespace DAL.Repositories
{
    public class CityRepositoty : IRepository<City>
    {
        private readonly IDbConnection _dbConnection;

		public CityRepositoty(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

        public Result Add(City entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("AddCity", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@name", entity.Name);
			cmd.Parameters["@name"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(City entity)
        {
            _dbConnection.OpenConnection();

			string query = "delete FROM Citys WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public IEnumerable<City> Find(string filter)
        {
            throw new NotImplementedException();
        }

        public City? FirstOrDefault(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<City> GetAll()
        {
            List<City> cities = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM Cities;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				cities.Add(new City
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("name"),
				}); ;
			}
			reader.Close();

			return cities;
        }

        public City? GetById(int id)
        {
			City? city = null;
            _dbConnection.OpenConnection();

			string query = $"SELECT * FROM Cities WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				city = new()
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("name"),
				};
			}
			reader.Close();

			return city;
        }

        public Result Update(City entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("UpdateCity", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@name", entity.Name);
			cmd.Parameters["@name"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }
}
}
