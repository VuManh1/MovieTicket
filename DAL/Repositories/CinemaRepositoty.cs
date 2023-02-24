using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;

namespace DAL.Repositories
{
    public class CinemaRepositoty : IRepository<Cinema>
    {
        private readonly IDbConnection _dbConnection;

		public CinemaRepositoty(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

        public Result Add(Cinema entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("AddCinema", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Name", entity.Name);
			cmd.Parameters["@Name"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@HallCount", entity.HallCount);
			cmd.Parameters["@HallCount"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@CityId", entity.City.Id);
			cmd.Parameters["@CityId"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(string id)
        {
            _dbConnection.OpenConnection();

			string query = "delete FROM Cinemas WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Cinema? FirstOrDefault(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cinema> GetAll()
        {
            List<Cinema> Cinema = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM Cinemas;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				Cinema.Add(new Cinema
				{
					Id = reader.GetString("id"),
					Name = reader.GetString("Name"),
					HallCount = reader.GetInt32("HallCount")
				}); ;
			}
			reader.Close();

			return Cinema;
        }

        public Cinema? GetById(string id)
        {
			Cinema? cinema = null;

            _dbConnection.OpenConnection();

			string query = "SELECT * FROM Cinemas WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				cinema = new Cinema
				{
					Id = reader.GetString("id"),
					Name = reader.GetString("Name"),
					HallCount = reader.GetInt32("HallCount")
				};
			}
			reader.Close();

			return cinema;
        }

        public Result Update(Cinema entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("UpdateCinema", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@name", entity.Name);
			cmd.Parameters["@name"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@HallCount", entity.HallCount);
			cmd.Parameters["@HallCount"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@CityId", entity.City.Id);
			cmd.Parameters["@CityId"].Direction = System.Data.ParameterDirection.Input;


			cmd.ExecuteNonQuery();

			return Result.OK();
        }
}
}
