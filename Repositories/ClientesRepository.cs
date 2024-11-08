
using Microsoft.Data.Sqlite;

public class ClientesRepository
{
    string connectionString;

    public ClientesRepository()
    {
        connectionString = @"Data Source = db/Tienda.db;Cache=Shared";
    }

    public List<Cliente> GetClientes()
    {
        List<Cliente> clientes = new List<Cliente>();
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"SELECT 
            ClienteId,
c           Nombre,
            Email,
            Telefono
        FROM 
            Cliente;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Cliente cliente = new Cliente();
                    cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                    cliente.Nombre = reader["Nombre"].ToString();
                    cliente.Email = reader["Email"].ToString();
                    cliente.Telefono = reader["Telefono"].ToString();
                    clientes.Add(cliente);
                }
            }
            connection.Close();
        }
        return clientes;
    }
}