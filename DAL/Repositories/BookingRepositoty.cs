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

        public Result Create(Booking entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("AddBooking", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@UserId", entity.User.Id);
			cmd.Parameters["@UserId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@ShowId", entity.Show.Id);
			cmd.Parameters["@ShowId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@SeatCount", entity.SeatCount);
			cmd.Parameters["@SeatCount"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@CreateTime", entity.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
			cmd.Parameters["@CreateTime"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@Total", entity.Total);
			cmd.Parameters["@Total"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@BookingId", MySqlDbType.Int32);
            cmd.Parameters["@BookingId"].Direction = System.Data.ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            int id = (int)cmd.Parameters["@BookingId"].Value;
            entity.Id = id;

            return Result.OK(id);
        }

		public Result UpdateShowSeat(Booking booking, List<Seat> seats)
		{
            _dbConnection.OpenConnection();

			string seatId = String.Join(",", seats.Select(s => s.Id));

            string query = "UPDATE showseat SET" +
                $" status = 'Picked', BookingId = {booking.Id}" +
				$" WHERE showseat.SeatId IN({seatId}) AND showseat.ShowId = {booking.Show.Id};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

			cmd.ExecuteNonQuery();

            return Result.OK();
		}

        public List<ShowSeat> GetShowSeats(Booking booking)
        {
            List<ShowSeat> showSeats = new();

            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM `ShowSeatDetails` WHERE BookingId = {booking.Id};";
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
                    Booking = booking,
                    SeatStatus = Enum.Parse<SeatStatus>(reader.GetString("ShowSeatStatus"))
                });
            }
            reader.Close();

            return showSeats;
        }

        public Result Delete(Booking entity)
        {
            _dbConnection.OpenConnection();

            string query = $"DELETE FROM bookings WHERE id = {entity.Id};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

            return Result.OK();
        }

        public IEnumerable<Booking> Find(string filter)
        {
            List<Booking>? booking = new();
            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM `BookingDetails` WHERE {filter};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                booking.Add(new Booking
                {
                    Id = reader.GetInt32("id"),
                    SeatCount = reader.GetInt32("SeatCount"),
                    CreateTime = reader.GetDateTime("CreateTime"),
                    Total = reader.GetInt32("total"),
                    Show = new Show
                    {
                        Id = reader.GetInt32("ShowId"),
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
                    },
                    User = new User
                    {
                        Id = reader.GetInt32("UserId"),
                        Name = reader.GetString("UserName"),
                        NormalizeName = reader.GetString("UserNormalizeName"),
                        Email = reader.GetString("UserEmail"),
                    }
                });
            }
            reader.Close();

            return booking;
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
					Id = reader.GetInt32("id"),
					SeatCount = reader.GetInt32("SeatCount"),
					CreateTime = reader.GetDateTime("CreateTime"),
					Total = reader.GetInt32("Total")
				}); ;
			}
			reader.Close();

			return Booking;
        }

        public Booking? GetById(int id)
        {
			Booking? booking = null;
            _dbConnection.OpenConnection();

			string query = $"SELECT * FROM `BookingDetails` WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			if(reader.HasRows)
			{
				reader.Read();

				booking = new Booking
				{
					Id = reader.GetInt32("id"),
					SeatCount = reader.GetInt32("SeatCount"),
					CreateTime = reader.GetDateTime("CreateTime"),
					Total = reader.GetInt32("total"),
					Show = new Show
                    {
                        Id = reader.GetInt32("ShowId"),
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
                    },
                    User = new User
                    {
                        Id = reader.GetInt32("UserId"),
                        Name = reader.GetString("UserName"),
                        NormalizeName = reader.GetString("UserNormalizeName"),
                        Email = reader.GetString("UserEmail"),
                    }
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
