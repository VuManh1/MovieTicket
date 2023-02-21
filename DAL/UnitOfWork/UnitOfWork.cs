using DAL.Repositories;
using MySql.Data.MySqlClient;
using SharedLibrary.DTO;

namespace DAL.UnitOfWork
{
	public interface IUnitOfWork
	{
		IRepository<User> UserRepository { get; }

		public MySqlTransaction BeginTransaction();
	}

	public class UnitOfWork : IUnitOfWork
	{
#nullable disable
		private readonly IDbConnection _dbConnection;

		private IRepository<User> _userRepository;

		public UnitOfWork(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

		public IDbConnection DbConnection
		{
			get { return _dbConnection; }
		}


		public IRepository<User> UserRepository
		{
			get
			{
				_userRepository ??= new UserRepository(_dbConnection);

				return _userRepository;
			}
		}

		public MySqlTransaction BeginTransaction()
		{
			_dbConnection.OpenConnection();

			return _dbConnection.Connection.BeginTransaction();
		}
	}
}
