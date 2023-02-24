using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;

namespace DAL.Repositories
{
    public class ShowRepositoty : IRepository<Show>
    {
        private readonly IDbConnection _dbConnection;

		public ShowRepositoty(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

        public Result Add(Show entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("AddShow", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@MovieId", entity.Movie.Id);
			cmd.Parameters["@MovieId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@HallId", entity.Hall.Id);
			cmd.Parameters["@HallId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@StartTime", entity.StartTime.ToString("yyyy-MM-dd"));
			cmd.Parameters["@StartTime"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Result Delete(string id)
        {
            _dbConnection.OpenConnection();

			string query = "delete FROM Shows WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public Show? FirstOrDefault(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Show> GetAll()
        {
            List<Show> Show = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM Shows;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				Show.Add(new Show
				{
					Id = reader.GetString("id"),
					StartTime = reader.GetDateTime("StartTime"),
				}); ;
			}
			reader.Close();

			return Show;
        }

        public Show? GetById(string id)
        {
			Show? show = null;
            _dbConnection.OpenConnection();

			string query = "SELECT * FROM Shows WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				show = new Show
				{
					Id = reader.GetString("id"),
					StartTime = reader.GetDateTime("StartTime"),
				};
			}
			reader.Close();

			return show;
        }

        public Result Update(Show entity)
        {
            _dbConnection.OpenConnection();

			MySqlCommand cmd = new("UpdateShow", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@MovieId", entity.Movie.Id);
			cmd.Parameters["@MovieId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@HallId", entity.Hall.Id);
			cmd.Parameters["@HallId"].Direction = System.Data.ParameterDirection.Input;

			cmd.Parameters.AddWithValue("@StartTime", entity.StartTime.ToString("yyyy-MM-dd"));
			cmd.Parameters["@StartTime"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
        }
}
}
