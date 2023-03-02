using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;

namespace DAL.Repositories
{
    public class SeatRepositoty : IRepository<Seat>
    {
        private readonly IDbConnection _dbConnection;

		public SeatRepositoty(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

        public Result Create(Seat entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("AddSeat", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@SeatNumber", entity.SeatNumber);
			cmd.Parameters["@SeatNumber"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@SeatType", entity.SeatType);
			cmd.Parameters["@SeatType"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@Price", entity.Price);
			cmd.Parameters["@Price"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Hall", entity.Hall.Id);
			cmd.Parameters["@Hall"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(Seat entity)
        {
            _dbConnection.OpenConnection();

			string query = "delete FROM Seats WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

		public string test()
		{
			_dbConnection.OpenConnection();

			string query = "SELECT * FROM Seats;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();


			return "";
		}

        public IEnumerable<Seat> GetAll()
        {
            List<Seat> Seat = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM Seats;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				Seat.Add(new Seat
				{
					Id = reader.GetInt32("id"),
					SeatNumber = reader.GetInt32("SeatNumber"),
					SeatType = Enum.Parse<SeatType>(reader.GetString("SeatType")),
                    Price = reader.GetDouble("Price")
				}); ;
			}
			reader.Close();

			return Seat;
        }

        public Seat? GetById(int id)
        {
			Seat? seat = null;
            _dbConnection.OpenConnection();

			string query = "SELECT * FROM Seats WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				seat = new Seat
				{
					Id = reader.GetInt32("id"),
					SeatNumber = reader.GetInt32("SeatNumber"),
					SeatType = Enum.Parse<SeatType>(reader.GetString("SeatType")),
                    Price = reader.GetDouble("Price")
				};
			}
			reader.Close();

			return seat;
        }

        public Result Update(Seat entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("UpdateSeat", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@SeatNumber", entity.SeatNumber);
			cmd.Parameters["@SeatNumber"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@SeatType", entity.SeatType);
			cmd.Parameters["@SeatType"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@Price", entity.Price);
			cmd.Parameters["@Price"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Hall", entity.Hall.Id);
			cmd.Parameters["@Hall"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Seat? FirstOrDefault(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seat> Find(string filter)
        {
            throw new NotImplementedException();
        }
    }
}
