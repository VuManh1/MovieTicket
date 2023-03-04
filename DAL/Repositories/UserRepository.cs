using MySql.Data.MySqlClient;
using SharedLibrary;
using SharedLibrary.DTO;

namespace DAL.Repositories
{
	public class UserRepository : IRepository<User>
	{
		private readonly IDbConnection _dbConnection;

		public UserRepository(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

		public Result Create(User entity)
		{
			_dbConnection.OpenConnection();

			MySqlCommand cmd = new("register", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@name", entity.Name);
			cmd.Parameters["@name"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@NormalizeName", entity.NormalizeName);
			cmd.Parameters["@NormalizeName"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);
			cmd.Parameters["@PasswordHash"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@salt", entity.Salt);
			cmd.Parameters["@salt"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@email", entity.Email);
			cmd.Parameters["@email"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@PhoneNumber", entity.PhoneNumber);
			cmd.Parameters["@PhoneNumber"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@CreateDate", entity.CreateDate?.ToString("yyyy-MM-dd"));
			cmd.Parameters["@CreateDate"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@IsLock", 0);
			cmd.Parameters["@IsLock"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@role", entity.Role.ToString());
			cmd.Parameters["@role"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@CityId", entity.City?.Id);
			cmd.Parameters["@CityId"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@UserId", MySqlDbType.Int32);
            cmd.Parameters["@UserId"].Direction = System.Data.ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            // get id of user
            int id = (int)cmd.Parameters["@UserId"].Value;
			entity.Id = id;

			return Result.OK(entity);
		}

		public Result Delete(User entity)
		{
            _dbConnection.OpenConnection();

            string query = $"DELETE FROM users WHERE id = {entity.Id};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            cmd.ExecuteNonQuery();

            return Result.OK();
        }

		public IEnumerable<User> GetAll()
		{
			List<User> users = new();

			_dbConnection.OpenConnection();

			string query = "SELECT * FROM `UserDetails`;";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				users.Add(new User
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("name"),
					NormalizeName = reader.GetString("NormalizeName"),
					PhoneNumber = reader["PhoneNumber"].GetType() != typeof(System.DBNull) ? reader.GetString("PhoneNumber") : null,
					CreateDate = DateOnly.FromDateTime(reader.GetDateTime("CreateDate")),
					Email = reader.GetString("email"),
					PasswordHash = reader.GetString("PasswordHash"),
					Salt = reader.GetString("salt"),
					IsLock = reader.GetInt32("IsLock") == 1,
					Role = Enum.Parse<Role>(reader.GetString("role")),
					City = reader["CityId"].GetType() != typeof(System.DBNull) ? 
						new()
						{
							Id = reader.GetInt32("CityId"),
							Name = reader.GetString("CityName"),
						} : null
				});
			}
			reader.Close();

			return users;
		}

		public User? FirstOrDefault(string filter)
		{
			User? user = null;

			_dbConnection.OpenConnection();

			string query = $"SELECT * FROM `UserDetails` WHERE {filter};";
			MySqlCommand cmd = new(query, _dbConnection.Connection);

			MySqlDataReader reader = cmd.ExecuteReader();

			if (reader.HasRows)
			{
				reader.Read();

				user = new User
				{
					Id = reader.GetInt32("id"),
					Name = reader.GetString("name"),
					NormalizeName = reader.GetString("NormalizeName"),
					PhoneNumber = reader["PhoneNumber"].GetType() != typeof(System.DBNull) ? reader.GetString("PhoneNumber") : null,
					CreateDate = DateOnly.FromDateTime(reader.GetDateTime("CreateDate")),
					Email = reader.GetString("email"),
					PasswordHash = reader.GetString("PasswordHash"),
					Salt = reader.GetString("salt"),
					IsLock = reader.GetInt32("IsLock") == 1,
					Role = Enum.Parse<Role>(reader.GetString("role")),
					City = reader["CityId"].GetType() != typeof(System.DBNull) ?
						new()
						{
							Id = reader.GetInt32("CityId"),
							Name = reader.GetString("CityName"),
						} : null
				};

			}
			reader.Close();

			return user;
		}

		public User? GetById(int id)
		{
            User? user = null;

            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM `UserDetails` WHERE id = {id};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                user = new User
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    NormalizeName = reader.GetString("NormalizeName"),
                    PhoneNumber = reader["PhoneNumber"].GetType() != typeof(System.DBNull) ? reader.GetString("PhoneNumber") : null,
                    CreateDate = DateOnly.FromDateTime(reader.GetDateTime("CreateDate")),
                    Email = reader.GetString("email"),
                    PasswordHash = reader.GetString("PasswordHash"),
                    Salt = reader.GetString("salt"),
                    IsLock = reader.GetInt32("IsLock") == 1,
                    Role = Enum.Parse<Role>(reader.GetString("role")),
                    City = reader["CityId"].GetType() != typeof(System.DBNull) ?
                        new()
                        {
                            Id = reader.GetInt32("CityId"),
                            Name = reader.GetString("CityName"),
                        } : null
                };

            }
            reader.Close();

            return user;
        }

		public Result Update(User entity)
		{
			_dbConnection.OpenConnection();

			MySqlCommand cmd = new("UpdateUser", _dbConnection.Connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			cmd.Parameters.AddWithValue("@id", entity.Id);
			cmd.Parameters["@id"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@newName", entity.Name);
			cmd.Parameters["@newName"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@newNormalizeName", entity.NormalizeName);
			cmd.Parameters["@newNormalizeName"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@newPasswordHash", entity.PasswordHash);
			cmd.Parameters["@newPasswordHash"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@newSalt", entity.Salt);
			cmd.Parameters["@newSalt"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@newEmail", entity.Email);
			cmd.Parameters["@newEmail"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@newPhoneNumber", entity.PhoneNumber);
			cmd.Parameters["@newPhoneNumber"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@newCreateDate", entity.CreateDate?.ToString("yyyy-MM-dd"));
			cmd.Parameters["@newCreateDate"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@newIsLock", entity.IsLock ? 1 : 0);
			cmd.Parameters["@newIsLock"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@newRole", entity.Role.ToString());
			cmd.Parameters["@newRole"].Direction = System.Data.ParameterDirection.Input;
			cmd.Parameters.AddWithValue("@CityId", entity.City?.Id);
			cmd.Parameters["@CityId"].Direction = System.Data.ParameterDirection.Input;

			cmd.ExecuteNonQuery();

			return Result.OK();
		}

        public IEnumerable<User> Find(string filter)
        {
            List<User> users = new();

            _dbConnection.OpenConnection();

            string query = $"SELECT * FROM `UserDetails` WHERE {filter};";
            MySqlCommand cmd = new(query, _dbConnection.Connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    NormalizeName = reader.GetString("NormalizeName"),
                    PhoneNumber = reader["PhoneNumber"].GetType() != typeof(System.DBNull) ? reader.GetString("PhoneNumber") : null,
                    CreateDate = DateOnly.FromDateTime(reader.GetDateTime("CreateDate")),
                    Email = reader.GetString("email"),
                    PasswordHash = reader.GetString("PasswordHash"),
                    Salt = reader.GetString("salt"),
                    IsLock = reader.GetInt32("IsLock") == 1,
                    Role = Enum.Parse<Role>(reader.GetString("role")),
                    City = reader["CityId"].GetType() != typeof(System.DBNull) ?
                        new()
                        {
                            Id = reader.GetInt32("CityId"),
                            Name = reader.GetString("CityName"),
                        } : null
                });
            }
            reader.Close();

            return users;
        }
    }
}
