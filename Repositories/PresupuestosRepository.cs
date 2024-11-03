
using Microsoft.Data.Sqlite;

class PresupuestosRepository
{
    public void CrearPresupuesto(Presupuesto presupuesto)
    {

        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) 
        VALUES (@destinatario, @fecha)";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@destinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion);
            command.ExecuteNonQuery();
            connection.Close();            
        }

    }
    public List<Presupuesto> ObtenerPresupuestos()
    {
        List<Presupuesto> presupuestos = new List<Presupuesto>();
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"SELECT 
            idPresupuesto,
            NombreDestinatario,
            FechaCreacion
        FROM 
            Presupuestos;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Presupuesto presupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]));
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
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"SELECT 
            P.idPresupuesto,
            P.NombreDestinatario,
            P.FechaCreacion,
            PR.idProducto,
            PR.Descripcion AS Producto,
            PR.Precio,
            PD.Cantidad
        FROM 
            Presupuestos P
        LEFT JOIN 
            PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
        LEFT JOIN 
            Productos PR ON PD.idProducto = PR.idProducto
        WHERE 
            P.idPresupuesto = @id;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            int cont = 1;
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    if(cont == 1)
                    {
                        presupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]));
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
        return presupuesto;
    }

    public void AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
    {
    
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresu, @idProd, @cant)";

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
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

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
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"UPDATE Presupuestos SET NombreDestinatario = @destinatario, FechaCreacion = @fecha WHERE idPresupuesto = @Id";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@destinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion);
            command.Parameters.AddWithValue("@Id", presupuesto.IdPresupuesto);
            command.ExecuteNonQuery();
            connection.Close();            
        }


    }

    public void EliminarPresupuestoPorId(int idPresupuesto)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

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