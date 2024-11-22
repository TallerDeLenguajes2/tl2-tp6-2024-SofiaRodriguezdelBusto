
using Microsoft.Data.Sqlite;

class PresupuestosRepository : IPresupuestoRepository
{
    private readonly string connectionString;
    public PresupuestosRepository(string CadenaDeConexion)
    {
        connectionString = CadenaDeConexion;
    }

    public void CrearPresupuesto(Presupuesto presupuesto)
    {

        string query = @"INSERT INTO Presupuestos (FechaCreacion, IdCliente) 
        VALUES (@fecha, @idC)";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@idC", presupuesto.Cliente.ClienteId);
            command.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion);
            command.ExecuteNonQuery();
            connection.Close();            
        }

    }
    public List<Presupuesto> ObtenerPresupuestos()
    {
        List<Presupuesto> presupuestos = new List<Presupuesto>();

        string query = @"SELECT 
            idPresupuesto,
            FechaCreacion,
            P.IdCliente,
            COALESCE(C.Nombre, 'No se asigno cliente') AS Cliente,
            C.Email,
            C.Telefono
        FROM 
            Presupuestos P
        LEFT JOIN
            Cliente C ON P.IdCliente = C.ClienteId;";
        
        Cliente cliente = new Cliente();

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if(!reader.IsDBNull(reader.GetOrdinal("IdCliente")))
                    {
                        cliente = new Cliente();
                        cliente.ClienteId = Convert.ToInt32(reader["IdCliente"]);
                        cliente.Nombre = reader["Cliente"].ToString();
                        cliente.Email =  reader["Email"].ToString();
                        cliente.Telefono = reader["Telefono"].ToString();
                    }
                    Presupuesto presupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), cliente, Convert.ToDateTime(reader["FechaCreacion"]));
                    presupuestos.Add(presupuesto);
                }
            }
            connection.Close();
        }
        return presupuestos;
    }

    public Presupuesto ObtenerPresupuestoPorId(int id)
    {
        Presupuesto presupuesto = null;

        string query = @"SELECT 
            P.idPresupuesto,
            P.IdCliente,
            P.FechaCreacion,
            PR.idProducto,
            PR.Descripcion AS Producto,
            PR.Precio,
            PD.Cantidad,
            COALESCE(C.Nombre, 'No se asigno cliente') AS cliente,
            C.Email,
            C.Telefono
        FROM 
            Presupuestos P
        LEFT JOIN 
            PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
        LEFT JOIN 
            Productos PR ON PD.idProducto = PR.idProducto
        LEFT JOIN Cliente C ON P.IdCliente = C.ClienteId
        WHERE 
            P.idPresupuesto = @id;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            int cont = 1;
            Cliente cliente = new Cliente();
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    if(cont == 1)
                    {
                        if(!reader.IsDBNull(reader.GetOrdinal("IdCliente")))
                        {
                            cliente = new Cliente();
                            cliente.ClienteId = Convert.ToInt32(reader["IdCliente"]);
                            cliente.Nombre = reader["Cliente"].ToString();
                            cliente.Email =  reader["Email"].ToString();
                            cliente.Telefono = reader["Telefono"].ToString();
                        }
                        presupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), cliente, Convert.ToDateTime(reader["FechaCreacion"]));
                    }
                    if(!reader.IsDBNull(reader.GetOrdinal("idProducto")))
                    {
                        Producto producto = new Producto(Convert.ToInt32(reader["idProducto"]), reader["Producto"].ToString(), Convert.ToInt32(reader["Precio"]));
                        PresupuestoDetalle detalle = new PresupuestoDetalle(producto,Convert.ToInt32(reader["Cantidad"]));
                        presupuesto.Detalle.Add(detalle);
                    }
                    cont++;
                }
            }
            connection.Close();
        }
        if (presupuesto == null)
        {
            throw new Exception("Presupuesto inexistente");
        }
        return presupuesto;
    }

    public void AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
    {

        string query =  $"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresu, @idProd, @cant) ON CONFLICT(idPresupuesto, idProducto) DO UPDATE SET Cantidad = Cantidad + @cant;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idPresu", idPresupuesto);
            command.Parameters.AddWithValue("@idProd", idProducto);
            command.Parameters.AddWithValue("@cant", cantidad);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void EliminarProducto(int idPresupuesto, int idProducto)
    {

        string query = @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @idP AND idProducto = @idPR";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idP", idPresupuesto);
            command.Parameters.AddWithValue("@idPR", idProducto);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void ModificarPresupuesto(Presupuesto presupuesto)
    {
        
        string query = @"UPDATE Presupuestos SET IdCliente = @idC, FechaCreacion = @fecha WHERE idPresupuesto = @Id";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@idC", presupuesto.Cliente.ClienteId);
            command.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion);
            command.Parameters.AddWithValue("@Id", presupuesto.IdPresupuesto);
            command.ExecuteNonQuery();
            connection.Close();            
        }


    }


    public void EliminarPresupuestoPorId(int idPresupuesto)
    {
    
        string query = @"DELETE FROM Presupuestos WHERE idPresupuesto = @IdP;";
        string query2 = @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @Id;";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            SqliteCommand command2 = new SqliteCommand(query2, connection);
            command.Parameters.AddWithValue("@IdP", idPresupuesto);
            command2.Parameters.AddWithValue("@Id", idPresupuesto);
            command2.ExecuteNonQuery();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

}
