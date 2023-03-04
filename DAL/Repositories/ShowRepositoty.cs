using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;
using System.Data;

namespace DAL.Repositories
{
    public class ShowRepositoty : IRepository<Show>
    {
        private readonly IDbConnection _dbConnection;

		public ShowRepositoty(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

        public Result Create(Show entity)
        {
            _dbConnection.OpenConnection();

            MySqlCommand cmd = new("AddShow", _dbConnection.Connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@HallId", entity.Hall.Id);
            cmd.Parameters["@HallId"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@MovieId", entity.Movie.Id);
            cmd.Parameters["@MovieId"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@StartTime", entity.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters["@StartTime"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@ShowId", MySqlDbType.Int32);
            cmd.Parameters["@ShowId"].Direction = System.Data.ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            // get id of show
            int id = (int)cmd.Parameters["@ShowId"].Value;
            entity.Id = id;

            return Result.OK(id);
        }

		public Result AddToShowSeat(Show show, List<Seat> seats)
		{
            _dbConnection.OpenConnection();

			string query = "INSERT INTO ShowSeat(ShowId, SeatId, BookingId, status) VALUES";

			seats.ForEach(s =>
			{
				query += $" ({show.Id}, {s.Id}, null, 'Empty'),";
			});

            // remove the ','
            query = query[0..^1];
            query += ";";

            MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

            return Result.OK();
        }

        public List<ShowSeat> GetShowSeats(Show show)
        {
            List<ShowSeat> showSeats = new();

            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM `ShowSeatDetails` WHERE ShowId = {show.Id};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                showSeats.Add(new ShowSeat()
                {
                    Seat = new()
                    {
                        Id = reader.GetInt32("SeatId"),
                        SeatNumber = reader.GetInt32("SeatNumber"),
                        Position = reader.GetInt32("position"),
                        SeatRow = reader.GetChar("SeatRow"),
                        SeatType = Enum.Parse<SeatType>(reader.GetString("type")),
                        Price = reader.GetDouble("price")
                    },
                    Show = show,
                    Booking = reader["BookingId"].GetType() != typeof(System.DBNull) ?
                        new Booking()
                        {
                            Id = reader.GetInt32("BookingId")
                        } :
                        null,
                    SeatStatus = Enum.Parse<SeatStatus>(reader.GetString("ShowSeatStatus"))
                });
            }
            reader.Close();

            return showSeats;
        }

        public Result Delete(Show entity)
        {
            _dbConnection.OpenConnection();

			string query = $"DELETE FROM Shows WHERE id = {entity.Id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public IEnumerable<Show> Find(string filter)
        {
            List<Show> shows = new();

            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM `ShowDetails` WHERE {filter};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                shows.Add(new Show
                {
                    Id = reader.GetInt32("id"),
                    StartTime = reader.GetDateTime("StartTime"),
                    Movie = new()
                    {
                        Id = reader.GetInt32("MovieId"),
                        Name = reader.GetString("MovieName"),
                        NormalizeName = reader.GetString("MovieNormalizeName"),
                        Description = reader["MovieDescription"].GetType() != typeof(System.DBNull) ? reader.GetString("MovieDescription") : null,
                        Length = reader.GetInt32("MovieLength"),
                        ReleaseDate = DateOnly.FromDateTime(reader.GetDateTime("MovieReleaseDate")),
                        Country = reader["MovieCountry"].GetType() != typeof(System.DBNull) ? reader.GetString("MovieCountry") : null,
                        MovieStatus = Enum.Parse<MovieStatus>(reader.GetString("MovieStatus")),

                    },
                    Hall = new()
                    {
                        Id = reader.GetInt32("HallId"),
                        Name = reader.GetString("HallName"),
                        SeatCount = reader.GetInt32("HallSeatCount"),
                        Height = reader.GetInt32("HallHeight"),
                        Width = reader.GetInt32("HallWidth"),
                        Cinema = new()
                        {
                            Id = reader.GetInt32("CinemaId"),
                            Name = reader.GetString("CinemaName"),
                            Address = reader["CinemaAddress"].GetType() != typeof(System.DBNull) ? reader.GetString("CinemaAddress") : null,
                            HallCount = reader.GetInt32("CinemaHallCount"),
                            City = new()
                            {
                                Id = reader.GetInt32("CId"),
                                Name = reader.GetString("CityName")
                            }
                        }
                    }
                });
            }
            reader.Close();

            return shows;
        }

        public Show? FirstOrDefault(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Show> GetAll()
        {
            List<Show> shows = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM `ShowDetails`;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				shows.Add(new Show
				{
					Id = reader.GetInt32("id"),
					StartTime = reader.GetDateTime("StartTime"),
                    Movie = new()
                    {
                        Id = reader.GetInt32("MovieId"),
                        Name = reader.GetString("MovieName"),
                        NormalizeName = reader.GetString("MovieNormalizeName"),
                        Description = reader["MovieDescription"].GetType() != typeof(System.DBNull) ? reader.GetString("MovieDescription") : null,
                        Length = reader.GetInt32("MovieLength"),
                        ReleaseDate = DateOnly.FromDateTime(reader.GetDateTime("MovieReleaseDate")),
                        Country = reader["MovieCountry"].GetType() != typeof(System.DBNull) ? reader.GetString("MovieCountry") : null,
                        MovieStatus = Enum.Parse<MovieStatus>(reader.GetString("MovieStatus")),

                    },
                    Hall = new()
                    {
                        Id = reader.GetInt32("HallId"),
                        Name = reader.GetString("HallName"),
                        SeatCount = reader.GetInt32("HallSeatCount"),
                        Height = reader.GetInt32("HallHeight"),
                        Width = reader.GetInt32("HallWidth"),
                        Cinema = new()
                        {
                            Id = reader.GetInt32("CinemaId"),
                            Name = reader.GetString("CinemaName"),
                            Address = reader["CinemaAddress"].GetType() != typeof(System.DBNull) ? reader.GetString("CinemaAddress") : null,
                            HallCount = reader.GetInt32("CinemaHallCount"),
                            City = new()
                            {
                                Id= reader.GetInt32("CId"),
                                Name = reader.GetString("CityName")
                            }
                        }
                    }
				});
			}
			reader.Close();

			return shows;
        }

        public Show? GetById(int id)
        {
            Show? show = null;

            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM `ShowDetails` WHERE id = {id};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                show = new Show
                {
                    Id = reader.GetInt32("id"),
                    StartTime = reader.GetDateTime("StartTime"),
                    Movie = new()
                    {
                        Id = reader.GetInt32("MovieId"),
                        Name = reader.GetString("MovieName"),
                        NormalizeName = reader.GetString("MovieNormalizeName"),
                        Description = reader["MovieDescription"].GetType() != typeof(System.DBNull) ? reader.GetString("MovieDescription") : null,
                        Length = reader.GetInt32("MovieLength"),
                        ReleaseDate = DateOnly.FromDateTime(reader.GetDateTime("MovieReleaseDate")),
                        Country = reader["MovieCountry"].GetType() != typeof(System.DBNull) ? reader.GetString("MovieCountry") : null,
                        MovieStatus = Enum.Parse<MovieStatus>(reader.GetString("MovieStatus")),

                    },
                    Hall = new()
                    {
                        Id = reader.GetInt32("HallId"),
                        Name = reader.GetString("HallName"),
                        SeatCount = reader.GetInt32("HallSeatCount"),
                        Height = reader.GetInt32("HallHeight"),
                        Width = reader.GetInt32("HallWidth"),
                        Cinema = new()
                        {
                            Id = reader.GetInt32("CinemaId"),
                            Name = reader.GetString("CinemaName"),
                            Address = reader["CinemaAddress"].GetType() != typeof(System.DBNull) ? reader.GetString("CinemaAddress") : null,
                            HallCount = reader.GetInt32("CinemaHallCount"),
                            City = new()
                            {
                                Id = reader.GetInt32("CId"),
                                Name = reader.GetString("CityName")
                            }
                        }
                    }
                };
            }
            reader.Close();

            return show;
        }

        public Result Update(Show entity)
        {
            _dbConnection.OpenConnection();

            string query = "UPDATE shows SET" +
                $" StartTime = '{entity.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                $" WHERE id = {entity.Id};";
            Console.WriteLine(query);
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

            return Result.OK();
        }
    }
}
