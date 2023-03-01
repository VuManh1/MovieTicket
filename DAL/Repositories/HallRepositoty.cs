using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;

namespace DAL.Repositories
{
    public class HallRepositoty : IRepository<Hall>
    {
        private readonly IDbConnection _dbConnection;

		public HallRepositoty(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

        public Result Add(Hall entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("AddHall", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@CinemaId", entity.Cinema.Id);
			cmd.Parameters["@CinemaId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Name", entity.Name);
			cmd.Parameters["@Name"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@SeatCount", entity.SeatCount);
			cmd.Parameters["@SeatCount"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(Hall entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Hall> Find(string filter)
        {
            throw new NotImplementedException();
        }

        public Hall? FirstOrDefault(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Hall> GetAll()
        {
            List<Hall> Hall = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM Halls;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				Hall.Add(new Hall
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("Name"),
					SeatCount = reader.GetInt32("SeatCount")
					
				}); ;
			}
			reader.Close();

			return Hall;
        }

        public Hall? GetById(int id)
        {
			Hall? hall = null;
            _dbConnection.OpenConnection();

			string query = "SELECT * FROM Halls WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				hall = new Hall
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("Name"),
					SeatCount = reader.GetInt32("SeatCount")
					
				};
			}
			reader.Close();

			return hall;
        }

        public Result Update(Hall entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("UpdateHall", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@CinemaId", entity.Cinema.Id);
			cmd.Parameters["@CinemaId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Name", entity.Name);
			cmd.Parameters["@Name"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@SeatCount", entity.SeatCount);
			cmd.Parameters["@SeatCount"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }
}
}
