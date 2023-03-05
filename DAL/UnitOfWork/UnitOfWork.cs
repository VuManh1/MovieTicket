using DAL.Repositories;
using MySql.Data.MySqlClient;
using SharedLibrary.DTO;

namespace DAL.UnitOfWork
{
	public interface IUnitOfWork
	{
		UserRepository UserRepository { get; }
		ShowRepositoty ShowRepository { get; }
		SeatRepositoty SeatRepository { get; }
		MovieRepositoty MovieRepository { get; }
		HallRepositoty HallRepository { get; }
		GenreRepositoty GenreRepository { get; }
		DirectorRepositoty DirectorRepository { get; }
		CityRepositoty CityRepository { get; }
		CastRepositoty CastRepository { get; }
		CinemaRepositoty CinemaRepository { get; }
		BookingRepositoty BookingRepository { get; }

		public void BeginTransaction();
		public void CommitTransaction();
		public void RollBack();
    }

	public class UnitOfWork : IUnitOfWork
	{
#pragma warning disable
		private MySqlTransaction? _transaction;
		private readonly IDbConnection _dbConnection;

		private UserRepository _userRepository;
		private ShowRepositoty _showRepository;

		private SeatRepositoty _seatRepository;

		private MovieRepositoty _movieRepository;

		private HallRepositoty _hallRepository;

		private GenreRepositoty _genreRepository;

		private DirectorRepositoty _directorRepository;

		private CityRepositoty _cityRepository;

		private CastRepositoty _castRepository;

		private CinemaRepositoty _cinemaRepository;

		private BookingRepositoty _bookingRepository;


		public UnitOfWork(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

		public IDbConnection DbConnection
		{
			get => _dbConnection; 
		}

		public void BeginTransaction()
		{
			_dbConnection.OpenConnection();

			_transaction = _dbConnection.Connection.BeginTransaction();
		}

        public void CommitTransaction()
        {
			if (_transaction == null) return;

			_transaction.Commit();
        }

        public void RollBack()
        {
            if (_transaction == null) return;

            _transaction.Rollback();
        }

        public UserRepository UserRepository
		{
			get
			{
				_userRepository ??= new UserRepository(_dbConnection);

				return _userRepository;
			}
		}

        public ShowRepositoty ShowRepository
		{
			get
			{
				_showRepository ??= new ShowRepositoty(_dbConnection);

				return _showRepository;
			}
		}

        public SeatRepositoty SeatRepository
		{
			get
			{
				_seatRepository ??= new SeatRepositoty(_dbConnection);

				return _seatRepository;
			}
		}

        public MovieRepositoty MovieRepository
		{
			get
			{
				_movieRepository ??= new MovieRepositoty(_dbConnection);
				
				return _movieRepository;
			}
		}

        public HallRepositoty HallRepository
		{
			get
			{
				_hallRepository ??= new HallRepositoty(_dbConnection);

				return _hallRepository;
			}
		}

        public GenreRepositoty GenreRepository
		{
			get
			{
				_genreRepository ??= new GenreRepositoty(_dbConnection);

				return _genreRepository;
			}
		}

        public DirectorRepositoty DirectorRepository
		{
			get
			{
				_directorRepository ??= new DirectorRepositoty(_dbConnection);

				return _directorRepository;
			}
		}

        public CityRepositoty CityRepository
		{
			get
			{
				_cityRepository ??= new CityRepositoty(_dbConnection);

				return _cityRepository;
			}
		}

        public CastRepositoty CastRepository
		{
			get
			{
				_castRepository ??= new CastRepositoty(_dbConnection);

				return _castRepository;
			}
		}

        public CinemaRepositoty CinemaRepository
		{
			get
			{
				_cinemaRepository ??= new CinemaRepositoty(_dbConnection);

				return _cinemaRepository;
			}
		}

        public BookingRepositoty BookingRepository
		{
			get
			{
				_bookingRepository ??= new BookingRepositoty(_dbConnection);

				return _bookingRepository;
			}
		}
    }
}
