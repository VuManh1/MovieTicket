using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;

namespace DAL.Repositories
{
    public class DirectorRepositoty : IRepository<Director>
    {
        private readonly IDbConnection _dbConnection;

		public DirectorRepositoty(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

        public Result Create(Director entity)
        {
            _dbConnection.OpenConnection();

            string query = $"INSERT INTO Directors(name, about) VALUES('{entity.Name}', '{entity.About}');";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

            return Result.OK();
        }

        public Result Delete(Director entity)
        {
            _dbConnection.OpenConnection();

			string query = $"DELETE FROM Directors WHERE id = {entity.Id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

			return Result.OK();
        }

        public IEnumerable<Director> Find(string filter)
        {
            List<Director> Director = new();

            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM Directors WHERE {filter};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Director.Add(new Director
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    About = reader["about"].GetType() != typeof(System.DBNull) ? reader.GetString("about") : null,
                });
            }
            reader.Close();

            return Director;
        }

        public Director? FirstOrDefault(string filter)
        {
            Director? director = null;
            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM Directors WHERE {filter};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                director = new Director
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    About = reader["about"].GetType() != typeof(System.DBNull) ? reader.GetString("about") : null,
                };
            }
            reader.Close();

            return director;
        }

        public IEnumerable<Director> GetAll()
        {
            List<Director> Director = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM Directors;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				Director.Add(new Director
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("Name"),
					About = reader["About"].GetType() != typeof(System.DBNull) ? reader.GetString("About") : null,
				}); ;
			}
			reader.Close();

			return Director;
        }

        public Director? GetById(int id)
        {
			Director? director = null;
            _dbConnection.OpenConnection();

			string query = $"SELECT * FROM Directors WHERE id = {id};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			if (reader.HasRows)
			{
				reader.Read();

				director = new Director
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("name"),
					About = reader["about"].GetType() != typeof(System.DBNull) ? reader.GetString("about") : null,
				};
			}
			reader.Close();

			return director;
        }

        public Result Update(Director entity)
        {
            _dbConnection.OpenConnection();

            string query = $"UPDATE Directors SET" +
                $" name = '{entity.Name}', about = '{entity.About}'" +
                $" WHERE id = {entity.Id};";

            MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

            return Result.OK();
        }
}
}
