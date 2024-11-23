using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Models; //Contains Product Class
using System.Data;

namespace Database
{
    // The ProductManager class provides methods for managing products in the database,
    // updating stock levels and fetching products.
    public class ProductManager 
    {
        // Updates the stock for a list of products. 
        // Decreases the stock count if the product uses stock and there is enough stock available.
        public static async Task UpdateStockAsync(List<Producto> productos)
        {
            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    foreach (var producto in productos)
                    {
                        if (producto.UsaStock == 1)
                        {
                            if (producto.Cantidad > producto.Existencia)
                            {
                                throw new Exception($"No hay suficiente stock para el producto con código {producto.Codigo}.");
                            }

                            string consulta = @"UPDATE productos 
                                        SET existencia = existencia - @CantidadVendida 
                                        WHERE codigo = @Codigo";

                            using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                            {
                                comando.Parameters.AddWithValue("@CantidadVendida", producto.Cantidad);
                                comando.Parameters.AddWithValue("@Codigo", producto.Codigo);

                                await comando.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la cantidad de productos: " + ex.Message);
            }
        }

        /// Retrieves products with zero stock ("existencia" = 0).
        public static class Get_Low_Stock_Products
        {
            public static async Task<List<Producto>> StockCeroAsync()
            {
                List<Producto> productos = new List<Producto>();

                try
                {
                    using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                    {
                        await conexion.OpenAsync();

                        string consulta = @"
                                SELECT 
                                    p.codigo AS Codigo, 
                                    p.nombre AS Nombre, 
                                    p.descripcion AS Descripcion, 
                                    p.precio AS Precio, 
                                    p.existencia AS Existencia, 
                                    p.usa_stock AS UsaStock, 
                                    m.desc_medidas AS Medida, 
                                    d.Nm_Dept AS Departamento
                                FROM productos p
                                JOIN medidas m ON p.id_medidas = m.id_medidas
                                JOIN departamentos d ON p.id_departamento = d.id_departamento
                                WHERE p.codigo = @codigo
                                LIMIT 1";

                        using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                        {
                            using (DbDataReader lector = await comando.ExecuteReaderAsync())
                            {
                                while (await lector.ReadAsync())
                                {
                                    // Creating a Producto object and populating its properties from the data reader (lector).
                                    // Null checks (IsDBNull) ensure default values are assigned if database fields are null.
                                    Producto producto = new Producto
                                    {
                                        Codigo = lector.IsDBNull(lector.GetOrdinal("codigo")) ? 0 : lector.GetInt32(lector.GetOrdinal("codigo")),
                                        Nombre = lector.IsDBNull(lector.GetOrdinal("nombre")) ? "N/A" : lector.GetString(lector.GetOrdinal("nombre")),
                                        Descripcion = lector.IsDBNull(lector.GetOrdinal("descripcion")) ? "N/A" : lector.GetString(lector.GetOrdinal("descripcion")),
                                        Precio = lector.IsDBNull(lector.GetOrdinal("precio")) ? 0.00m : lector.GetDecimal(lector.GetOrdinal("precio")),
                                        Existencia = lector.IsDBNull(lector.GetOrdinal("existencia")) ? 0 : lector.GetInt32(lector.GetOrdinal("existencia")),
                                        Medida = lector.IsDBNull(lector.GetOrdinal("desc_medidas")) ? "N/A" : lector.GetString(lector.GetOrdinal("desc_medidas")),
                                        Departamento = lector.IsDBNull(lector.GetOrdinal("desc_departamento")) ? "N/A" : lector.GetString(lector.GetOrdinal("desc_departamento")),
                                        Cantidad = 0,  // Set to zero as a default value, required to initialize the product object.
                                        UsaStock = lector.IsDBNull(lector.GetOrdinal("usa_stock")) ? 0 : lector.GetInt32(lector.GetOrdinal("usa_stock"))
                                    };

                                    productos.Add(producto);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los productos con stock cero: " + ex.Message);
                }

                return productos;
            }
        }

        // Retrieves a single product by its code.
        public static async Task<Producto?> GetProductByCodeAsync(int codigo)
        {
            Producto? product = null;
            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    string consulta = @"
                        SELECT 
                            p.codigo, 
                            p.nombre, 
                            p.descripcion, 
                            p.precio, 
                            p.existencia, 
                            p.usa_stock, 
                            m.desc_medidas, 
                            d.Nm_Dept AS desc_departamento
                        FROM productos p
                        JOIN medidas m ON p.id_medidas = m.id_medidas
                        JOIN departamentos d ON p.id_departamento = d.id_departamento
                        WHERE p.codigo = @codigo
                        LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(consulta, conexion))
                    {
                        command.Parameters.AddWithValue("@codigo", codigo);

                        using (DbDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                product = new Producto
                                {
                                    // Creating a Producto object and populating its properties from the data reader (lector).
                                    // Null checks (IsDBNull) ensure default values are assigned if database fields are null.
                                    Codigo = reader.GetInt32("codigo"),
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? "N/A" : reader.GetString("nombre"),
                                    Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? "N/A" : reader.GetString("descripcion"),
                                    Precio = reader.IsDBNull(reader.GetOrdinal("precio")) ? 0.00m : reader.GetDecimal("precio"),
                                    Existencia = reader.IsDBNull(reader.GetOrdinal("existencia")) ? 0 : reader.GetInt32("existencia"),
                                    UsaStock = reader.IsDBNull(reader.GetOrdinal("usa_stock")) ? 0 : reader.GetInt32("usa_stock"),
                                    Medida = reader.IsDBNull(reader.GetOrdinal("desc_medidas")) ? "N/A" : reader.GetString("desc_medidas"),
                                    Departamento = reader.IsDBNull(reader.GetOrdinal("desc_departamento")) ? "PRUEBA" : reader.GetString("desc_departamento"),
                                    Cantidad = 0 // Set to zero as a default value, required to initialize the product object.
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving product by code: {ex.Message}");
            }

            return product;
        }

        // Retrieves a list of products with names similar to the given input.
        public static async Task<List<Producto>> GetProductsByNameAsync(string name)
        {
            List<Producto> products = new List<Producto>();

            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    string consulta = @"
                SELECT 
                    p.codigo AS Codigo, 
                    p.nombre AS Nombre, 
                    p.descripcion AS Descripcion, 
                    p.precio AS Precio, 
                    p.existencia AS Existencia, 
                    p.usa_stock AS UsaStock, 
                    m.desc_medidas AS Medida, 
                    d.Nm_Dept AS Departamento
                FROM productos p
                JOIN medidas m ON p.id_medidas = m.id_medidas
                JOIN departamentos d ON p.id_departamento = d.id_departamento
                WHERE p.nombre LIKE @name";

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        // Use '%' for SQL wildcard matching
                        comando.Parameters.AddWithValue("@name", $"%{name}%");

                        using (DbDataReader lector = await comando.ExecuteReaderAsync())
                        {
                            while (await lector.ReadAsync())
                            {
                                Producto product = new Producto
                                {
                                    // Creating a Producto object and populating its properties from the data reader (lector).
                                    // Null checks (IsDBNull) ensure default values are assigned if database fields are null.
                                    Codigo = lector.GetInt32("Codigo"),
                                    Nombre = lector.IsDBNull(lector.GetOrdinal("Nombre")) ? "N/A" : lector.GetString("Nombre"),
                                    Descripcion = lector.IsDBNull(lector.GetOrdinal("Descripcion")) ? "N/A" : lector.GetString("Descripcion"),
                                    Precio = lector.IsDBNull(lector.GetOrdinal("Precio")) ? 0.00m : lector.GetDecimal("Precio"),
                                    Existencia = lector.IsDBNull(lector.GetOrdinal("Existencia")) ? 0 : lector.GetInt32("Existencia"),
                                    UsaStock = lector.IsDBNull(lector.GetOrdinal("UsaStock")) ? 0 : lector.GetInt32("UsaStock"),
                                    Medida = lector.IsDBNull(lector.GetOrdinal("Medida")) ? "N/A" : lector.GetString("Medida"),
                                    Departamento = lector.IsDBNull(lector.GetOrdinal("Departamento")) ? "N/A" : lector.GetString("Departamento"),
                                    Cantidad = 0  // Set to zero as a default value, required to initialize the product object.
                                };

                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving products by name: " + ex.Message);
            }

            return products;
        }
    }
}
