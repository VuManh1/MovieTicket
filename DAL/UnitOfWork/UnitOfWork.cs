using DAL.Repositories;
using MySql.Data.MySqlClient;
using SharedLibrary.DTO;

namespace DAL.UnitOfWork
{
	public interface IUnitOfWork
	{
		IRepository<User> UserRepository { get; }
		IRepository<ShowSeat> ShowSeatRepository { get; }
		IRepository<Show> ShowRepository { get; }
		IRepository<Seat> SeatRepository { get; }
		IRepository<Movie> MovieRepository { get; }
		IRepository<Hall> HallRepository { get; }
		IRepository<Genre> GenreRepository { get; }
		IRepository<Director> DirectorRepository { get; }
		IRepository<City> CityRepository { get; }
		IRepository<Cast> CastRepository { get; }
		IRepository<Cinema> CinemaRepository { get; }
		IRepository<Booking> BookingRepository { get; }


		public MySqlTransaction BeginTransaction();
	}

	public class UnitOfWork : IUnitOfWork
	{
#nullable disable
		private readonly IDbConnection _dbConnection;

		private IRepository<User> _userRepository;
		private IRepository<ShowSeat> _showSeatRepository;
		private IRepository<Show> _showRepository;

		private IRepository<Seat> _seatRepository;

		private IRepository<Movie> _movieRepository;

		private IRepository<Hall> _hallRepository;

		private IRepository<Genre> _genreRepository;

		private IRepository<Director> _directorRepository;

		private IRepository<City> _cityRepository;

		private IRepository<Cast> _castRepository;

		private IRepository<Cinema> _cinemaRepository;

		private IRepository<Booking> _bookingRepository;



		public UnitOfWork(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

		public IDbConnection DbConnection
		{
			get { return _dbConnection; }
		}

		public MySqlTransaction BeginTransaction()
		{
			_dbConnection.OpenConnection();

			return _dbConnection.Connection.BeginTransaction();
		}

        MySqlTransaction IUnitOfWork.BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public IRepository<User> UserRepository
		{
			get
			{
				_userRepository ??= new UserRepository(_dbConnection);

				return _userRepository;
			}
		}

        public IRepository<ShowSeat> ShowSeatRepository
		{
			get
			{
				_showSeatRepository ??= new ShowSeatRepositoty(_dbConnection);

				return _showSeatRepository;
			}
		}

        public IRepository<Show> ShowRepository
		{
			get
			{
				_showRepository ??= new ShowRepositoty(_dbConnection);

				return _showRepository;
			}
		}

        public IRepository<Seat> SeatRepository
		{
			get
			{
				_seatRepository ??= new SeatRepositoty(_dbConnection);

				return _seatRepository;
			}
		}

        public IRepository<Movie> MovieRepository
		{
			get
			{
				_movieRepository ??= new MovieRepositoty(_dbConnection);

				return _movieRepository;
			}
		}

        public IRepository<Hall> HallRepository
		{
			get
			{
				_hallRepository ??= new HallRepositoty(_dbConnection);

				return _hallRepository;
			}
		}

        public IRepository<Genre> GenreRepository
		{
			get
			{
				_genreRepository ??= new GenreRepositoty(_dbConnection);

				return _genreRepository;
			}
		}

        public IRepository<Director> DirectorRepository
		{
			get
			{
				_directorRepository ??= new DirectorRepositoty(_dbConnection);

				return _directorRepository;
			}
		}

        public IRepository<City> CityRepository
		{
			get
			{
				_cityRepository ??= new CityRepositoty(_dbConnection);

				return _cityRepository;
			}
		}

        public IRepository<Cast> CastRepository
		{
			get
			{
				_castRepository ??= new CastRepositoty(_dbConnection);

				return _castRepository;
			}
		}

        public IRepository<Cinema> CinemaRepository
		{
			get
			{
				_cinemaRepository ??= new CinemaRepositoty(_dbConnection);

				return _cinemaRepository;
			}
		}

        public IRepository<Booking> BookingRepository
		{
			get
			{
				_bookingRepository ??= new BookingRepositoty(_dbConnection);

				return _bookingRepository;
			}
		}
    }
}
