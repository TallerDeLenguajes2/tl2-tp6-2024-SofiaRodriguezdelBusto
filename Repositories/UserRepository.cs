

using Microsoft.Data.Sqlite;

public class UserRepository : IUserRepository
{
    private readonly string connectionString;

    public UserRepository(string CadenaDeConexion)
    {
        connectionString = CadenaDeConexion;
    }

    public User GetUser(string username, string password)
    {
        User user = null;

        string query = @"SELECT * FROM Usuario WHERE usuario = @username AND password = @contra ";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@contra", password);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User();
                    user.Id = Convert.ToInt32(reader["id"]);
                    user.Nombre = reader["nombre"].ToString();
                    user.Username = reader["usuario"].ToString();
                    user.Password = reader["password"].ToString();
                    user.AccessLevel = (AccessLevel)Convert.ToInt32(reader["id_rol"]);;
                }

            }
            connection.Close();            
        }
        return user;
    }

    public void AltaUsuario(User usuario)
    {
        string query = @"INSERT INTO Usuario (nombre, usuario, password, id_rol) VALUES (@nombre, @usu, @contra, @rol)";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@nombre", usuario.Nombre);
            command.Parameters.AddWithValue("@usu", usuario.Username);
            command.Parameters.AddWithValue("@contra", usuario.Password);
            command.Parameters.AddWithValue("@rol", (int)usuario.AccessLevel);
            command.ExecuteNonQuery();
            connection.Close();            
        }

    }

}
