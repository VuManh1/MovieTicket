using MySql.Data.MySqlClient;

namespace DAL
{
	public interface IDbConnection
	{
		public MySqlConnection Connection { get; }
		public MySqlConnection OpenConnection();
		public void CloseConnection();
	}

	public class DbConnection : IDbConnection
	{
#nullable disable
		private readonly string _connectionString;
		private MySqlConnection _connection;

		public DbConnection(string connectionString)
		{
			_connectionString = connectionString;
		}

		public MySqlConnection Connection 
		{ 
			get
			{
				_connection ??= new MySqlConnection
					{
						ConnectionString = _connectionString,
					};

				return _connection;
			}
		}

		public MySqlConnection OpenConnection()
		{
			if (Connection.State == System.Data.ConnectionState.Open) return Connection;

			Connection.Open();
			return Connection;
		}

		public void CloseConnection()
		{
			if (Connection.State == System.Data.ConnectionState.Closed) return;

			Connection.Close();
		}
	}
}