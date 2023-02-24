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

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Name", entity.Name);
			cmd.Parameters["@Name"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@State", entity.State);
			cmd.Parameters["@State"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(string id)
        {
            _dbConnection.OpenConnection();

			string query = "delete FROM Citys WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public City? FirstOrDefault(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<City> GetAll()
        {
            List<City> City = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM Citys;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				City.Add(new City
				{
					Id = reader.GetString("id"),
					Name = reader.GetString("Name"),
					State = reader.GetString("State")
				}); ;
			}
			reader.Close();

			return City;
        }

        public City? GetById(string id)
        {
			City? city = null;
            _dbConnection.OpenConnection();

			string query = "SELECT * FROM Citys WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				city = new City
				{
					Id = reader.GetString("id"),
					Name = reader.GetString("Name"),
					State = reader.GetString("State")
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

			cmd.Parameters.AddWithValue("@State", entity.State);
			cmd.Parameters["@State"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }
}
}
