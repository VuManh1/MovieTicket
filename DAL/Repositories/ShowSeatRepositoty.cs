using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;

namespace DAL.Repositories
{
    public class ShowSeatRepositoty : IRepository<ShowSeat>
    {
        private readonly IDbConnection _dbConnection;

		public ShowSeatRepositoty(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

        public Result Add(ShowSeat entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("AddShowSeat", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@SeatId", entity.Seat.Id);
			cmd.Parameters["@SeatId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@ShowId", entity.Show.Id);
			cmd.Parameters["@ShowId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@BookingId", entity.Booking.Id);
			cmd.Parameters["@BookingId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@SeatStatus", entity.SeatStatus);
			cmd.Parameters["@SeatStatus"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(ShowSeat entity)
        {
            _dbConnection.OpenConnection();

			string query = "delete FROM ShowSeats WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public IEnumerable<ShowSeat> Find(string filter)
        {
            throw new NotImplementedException();
        }

        public ShowSeat? FirstOrDefault(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ShowSeat> GetAll()
        {
            List<ShowSeat> ShowSeat = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM ShowSeats;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				ShowSeat.Add(new ShowSeat
				{
					
					SeatStatus = Enum.Parse<SeatStatus>(reader.GetString("SeatStatus")),
				}); ;
			}
			reader.Close();

			return ShowSeat;
        }

        public ShowSeat? GetById(int id)
        {
			ShowSeat? showSeat = null;
            _dbConnection.OpenConnection();

			string query = "SELECT * FROM ShowSeats WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				showSeat =new ShowSeat
				{
					
					SeatStatus = Enum.Parse<SeatStatus>(reader.GetString("SeatStatus")),
				};
			}
			reader.Close();

			return showSeat;
        }

        public Result Update(ShowSeat entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("UpdateShowSeat", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@SeatId", entity.Seat.Id);
			cmd.Parameters["@SeatId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@ShowId", entity.Show.Id);
			cmd.Parameters["@ShowId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@BookingId", entity.Booking.Id);
			cmd.Parameters["@BookingId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@SeatStatus", entity.SeatStatus);
			cmd.Parameters["@SeatStatus"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }
}
}
