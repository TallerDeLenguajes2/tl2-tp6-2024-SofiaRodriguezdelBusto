
using System.Threading.Channels;
using Microsoft.Data.Sqlite;

public class ClientesRepository
{
    string connectionString;

    public ClientesRepository()
    {
        connectionString = @"Data Source = db/Tienda.db;Cache=Shared";
    }


    public void CrearCliente(AltaClienteViewModel cliente)
    {
        string query = @"INSERT INTO Cliente (Nombre, Email, Telefono) VALUES (@nombre, @email, @telefono);";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@email", cliente.Email);
            command.Parameters.AddWithValue("@telefono", cliente.Telefono);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Cliente> ObtenerClientes()
    {
        List<Cliente> clientes = new List<Cliente>();

        string query = "SELECT * FROM Cliente";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Cliente nuevoCliente = new Cliente();
                        nuevoCliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                        nuevoCliente.Nombre = reader["Nombre"].ToString();
                        nuevoCliente.Email = reader["Email"].ToString();
                        nuevoCliente.Telefono = reader["Telefono"].ToString();
                        clientes.Add(nuevoCliente);
                    }
                }

            }
            connection.Close();
        }
        return clientes;
    }

    public void ModificarCliente(ModificarClienteViewModel cliente)
    {
        string query = @"UPDATE Cliente SET Nombre = @nombre, Email = @email, Telefono = @telefono WHERE ClienteId = @Id;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@email", cliente.Email);
            command.Parameters.AddWithValue("@telefono", cliente.Telefono);
            command.Parameters.AddWithValue("@Id", cliente.ClienteId);
            command.ExecuteNonQuery();
            connection.Close();
        }

    }

    public Cliente ObtenerCliente(int id)
    {
        Cliente cliente = null; //Uso el null para devolver en caso de no encontrar nada

        string query = @"SELECT * FROM Cliente WHERE ClienteId = @id;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    cliente = new Cliente();
                    cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                    cliente.Nombre = reader["Nombre"].ToString();
                    cliente.Email = reader["Email"].ToString();
                    cliente.Telefono = reader["Telefono"].ToString();
                }
            }
            connection.Close();
        }
        return cliente;
    }

    public void EliminarCliente(int id)
    {
        string query = @"DELETE FROM Cliente WHERE ClienteId = @Id;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
