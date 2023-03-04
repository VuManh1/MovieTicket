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

        public Result Create(Hall entity)
        {
            _dbConnection.OpenConnection();

            string query = $"INSERT INTO halls(name, CinemaId, width, height) VALUES" +
                $"('{entity.Name}', {entity.Cinema.Id}, {entity.Width}, {entity.Height});";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

            return Result.OK();
        }

        public Result Delete(Hall entity)
        {
            _dbConnection.OpenConnection();

            string query = $"DELETE FROM Halls WHERE id = {entity.Id};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

            return Result.OK();
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
					
				});
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

            string query = $"UPDATE Halls SET" +
                $" name = '{entity.Name}', SeatCount = {entity.SeatCount}, CinemaId = {entity.Cinema.Id}," +
                $" width = {entity.Width}, height = {entity.Height}" +
                $" WHERE id = {entity.Id};";

            MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

            return Result.OK();
        }

        public IEnumerable<Seat> GetSeats(Hall hall)
        {
            List<Seat> Seat = new();

            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM Seats WHERE HallId = {hall.Id};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Seat.Add(new Seat
                {
                    Id = reader.GetInt32("id"),
                    Position = reader.GetInt32("position"),
                    SeatRow = reader.GetChar("SeatRow"),
                    SeatNumber = reader.GetInt32("SeatNumber"),
                    SeatType = Enum.Parse<SeatType>(reader.GetString("type")),
                    Price = reader.GetDouble("price"),
                    Hall = hall
                });
            }
            reader.Close();

            return Seat;
        }
    }
}
