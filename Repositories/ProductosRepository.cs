
using Microsoft.Data.Sqlite;

class ProductosRepository
{
    public void CrearProducto(Producto producto)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"INSERT INTO Productos (Descripcion, Precio) 
        VALUES (@Descripcion, @Precio)";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.ExecuteNonQuery();
            connection.Close();            
        }

    }

    public List<Producto>  ObtenerProductos()
    {
        List<Producto> productos = new List<Producto>();
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"SELECT * FROM Productos";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    Producto nuevoProducto = new Producto();
                    nuevoProducto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    nuevoProducto.Descripcion = reader["Descripcion"].ToString();
                    nuevoProducto.Precio = Convert.ToInt32(reader["Precio"]);
                    productos.Add(nuevoProducto);

                }
                
            }
            connection.Close();            
        }

        return productos;

    }


    public void ModificarProducto(Producto producto)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @Id";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.Parameters.AddWithValue("@Id", producto.IdProducto);
            command.ExecuteNonQuery();
            connection.Close();            
        }

    }

    public Producto  ObtenerProductoPorId(int id)
    {
        Producto producto = null;
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"SELECT * FROM Productos WHERE idProducto = @id ";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@id", id);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    producto = new Producto();
                    producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);
                }

            }
            connection.Close();            
        }
        return producto;
    }

    public void EliminarProductoPorId(int id)
    {
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"DELETE FROM Productos WHERE idProducto = @Id;";
        string query2 = @"DELETE FROM PresupuestosDetalle WHERE idProducto = @id;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            SqliteCommand command2 = new SqliteCommand(query2,connection);
            command.Parameters.AddWithValue("@Id", id);
            command2.Parameters.AddWithValue("@id", id);
            command2.ExecuteNonQuery();
            command.ExecuteNonQuery();
            connection.Close();            
        }
    }

    

}