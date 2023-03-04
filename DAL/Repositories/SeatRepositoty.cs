using MySql.Data.MySqlClient;
using Mysqlx.Crud;
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

            string query = $"INSERT INTO seats(SeatRow, SeatNumber, position, type, price, HallId) VALUES" +
                $"('{entity.SeatRow}', {entity.SeatNumber}, {entity.Position}, '{entity.SeatType}'," +
				$" @price, {entity.Hall.Id});";

            MySqlCommand cmd = new(query, _dbConnection.Connection);
            cmd.Parameters.AddWithValue("@price", entity.Price);
            cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();
            cmd.CommandText = "SELECT MAX(id) FROM seats;";
            
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();

            int seatid = reader.GetInt32("MAX(id)");
            entity.Id = seatid;

            reader.Close();

            return Result.OK(seatid);
        }

        public Result Delete(Seat entity)
        {
            _dbConnection.OpenConnection();

			string query = $"delete FROM Seats WHERE id = {entity.Id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
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
					Position = reader.GetInt32("position"),
                    SeatRow = reader.GetChar("SeatRow"),
					SeatNumber = reader.GetInt32("SeatNumber"),
					SeatType = Enum.Parse<SeatType>(reader.GetString("SeatType")),
                    Price = reader.GetDouble("price")
				});
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

            string query = $"UPDATE Seats SET" +
                $" SeatRow = '{entity.SeatRow}', SeatNumber = {entity.SeatNumber}, position = {entity.Position}," +
                $" type = '{entity.SeatType}', price = {entity.Price}, HallId = {entity.Hall.Id}" +
                $" WHERE id = {entity.Id};";

            MySqlCommand cmd = new(query, _dbConnection.Connection);

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
