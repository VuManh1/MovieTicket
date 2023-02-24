using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;

namespace DAL.Repositories
{
    public class BookingRepositoty : IRepository<Booking>
    {
        private readonly IDbConnection _dbConnection;

		public BookingRepositoty(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

        public Result Add(Booking entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("AddBooking", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@UserId", entity.User.Id);
			cmd.Parameters["@UserId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@ShowId", entity.Show.Id);
			cmd.Parameters["@ShowId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@SeatCount", entity.SeatCount);
			cmd.Parameters["@SeatCount"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@CreateTime", entity.CreateTime.ToString("yyyy-MM-dd"));
			cmd.Parameters["@CreateTime"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Total", entity.Total);
			cmd.Parameters["@Total"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(string id)
        {
            _dbConnection.OpenConnection();

			string query = "delete FROM Bookings WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Booking? FirstOrDefault(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Booking> GetAll()
        {
            List<Booking> Booking = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM Bookings;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				Booking.Add(new Booking
				{
					Id = reader.GetString("id"),
					SeatCount = reader.GetInt32("SeatCount"),
					CreateTime = reader.GetDateTime("CreateTime"),
					Total = reader.GetInt32("Total")
				}); ;
			}
			reader.Close();

			return Booking;
        }

        public Booking? GetById(string id)
        {

			Booking? booking = null;
            _dbConnection.OpenConnection();

			string query = $"SELECT * FROM Bookings WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				booking = new Booking
				{
					Id = reader.GetString("id"),
					SeatCount = reader.GetInt32("SeatCount"),
					CreateTime = reader.GetDateTime("CreateTime"),
					Total = reader.GetInt32("Total")
				};
			}
			reader.Close();

			return booking;
        }

        public Result Update(Booking entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("UpdateBooking", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@UserId", entity.User.Id);
			cmd.Parameters["@UserId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@ShowId", entity.Show.Id);
			cmd.Parameters["@ShowId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@SeatCount", entity.SeatCount);
			cmd.Parameters["@SeatCount"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@CreateTime", entity.CreateTime.ToString("yyyy-MM-dd"));
			cmd.Parameters["@CreateTime"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Total", entity.Total);
			cmd.Parameters["@Total"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }
}
}
