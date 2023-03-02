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

        public Result Create(Cinema entity)
        {
            _dbConnection.OpenConnection();

            string query = "INSERT INTO cinemas(name, HallCount, address, CityId) VALUES" +
                $"('{entity.Name}', {entity.HallCount}, '{entity.Address}', {entity.City?.Id})";

            MySqlCommand cmd = new(query, _dbConnection.Connection);

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(Cinema entity)
        {
            _dbConnection.OpenConnection();

			string query = $"DELETE FROM Cinemas WHERE id = {entity.Id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public IEnumerable<Cinema> Find(string filter)
        {
            List<Cinema> cinemas = new();

            _dbConnection.OpenConnection();

            string query = "SELECT cinemas.id, cinemas.name, cinemas.address, cinemas.HallCount," +
                " cities.id AS CId, cities.name AS CityName FROM Cinemas" +
                " JOIN cities" +
                " ON cinemas.CityId = cities.id" +
                $" WHERE {filter};";

            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                cinemas.Add(new Cinema
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    HallCount = reader.GetInt32("HallCount"),
                    Address = reader["address"].GetType() != typeof(System.DBNull) ? reader.GetString("address") : null,
                    City = new()
                    {
                        Id = reader.GetInt32("CId"),
                        Name = reader.GetString("CityName")
                    }
                });
            }
            reader.Close();

            return cinemas;
        }

        public Cinema? FirstOrDefault(string filter)
        {
            Cinema? cinema = null;

            _dbConnection.OpenConnection();

            string query = "SELECT cinemas.id, cinemas.name, cinemas.address, cinemas.HallCount," +
                " cities.id AS CId, cities.name AS CityName FROM Cinemas" +
                " JOIN cities" +
                " ON cinemas.CityId = cities.id" +
                $" WHERE {filter};";

            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                cinema = new Cinema
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    HallCount = reader.GetInt32("HallCount"),
                    Address = reader["address"].GetType() != typeof(System.DBNull) ? reader.GetString("address") : null,
                    City = new()
                    {
                        Id = reader.GetInt32("CId"),
                        Name = reader.GetString("CityName")
                    }
                };
            }
            reader.Close();

            return cinema;
        }

        public IEnumerable<Cinema> GetAll()
        {
            List<Cinema> cinemas = new();

			_dbConnection.OpenConnection();

			string query = "SELECT cinemas.id, cinemas.name, cinemas.address, cinemas.HallCount," +
				" cities.id AS CId, cities.name AS CityName FROM Cinemas" +
				" JOIN cities" +
				" ON cinemas.CityId = cities.id;";

            MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				cinemas.Add(new Cinema
				{
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    HallCount = reader.GetInt32("HallCount"),
                    Address = reader["address"].GetType() != typeof(System.DBNull) ? reader.GetString("address") : null,
                    City = new()
                    {
                        Id = reader.GetInt32("CId"),
                        Name = reader.GetString("CityName")
                    }
                });
			}
			reader.Close();

			return cinemas;
        }

        public Cinema? GetById(int id)
        {
			Cinema? cinema = null;

            _dbConnection.OpenConnection();

			string query = "SELECT cinemas.id, cinemas.name, cinemas.address, cinemas.HallCount," +
				" cities.id AS CId, cities.name AS CityName FROM Cinemas" +
				" JOIN cities" +
                " ON cinemas.CityId = cities.id" +
				$" WHERE cinemas.id = {id};";

			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			if (reader.HasRows)
			{
                reader.Read();

				cinema = new Cinema
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("name"),
					HallCount = reader.GetInt32("HallCount"),
					Address = reader["address"].GetType() != typeof(System.DBNull) ? reader.GetString("address") : null,
					City = new()
					{
						Id = reader.GetInt32("CId"),
						Name = reader.GetString("CityName")
					}
                };
			}
			reader.Close();

			return cinema;
        }

        public Result Update(Cinema entity)
        {
            _dbConnection.OpenConnection();

            string query = "UPDATE cinemas SET" +
                $" name = '{entity.Name}', HallCount = {entity.HallCount}, address = '{entity.Address}', CityId = {entity.City?.Id}" +
                $" WHERE id = {entity.Id};";

            MySqlCommand cmd = new(query, _dbConnection.Connection);

			cmd.ExecuteNonQuery();

			return Result.OK();
        }
}
}
