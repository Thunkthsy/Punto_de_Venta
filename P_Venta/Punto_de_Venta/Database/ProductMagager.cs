using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Models;
using System.Data;

namespace Database
{
    public class ProductManager
    {
        /// Asynchronously retrieves a list of products from the database.
        /// <returns>A list of Producto objects representing the products.
        public static async Task<List<Producto>> GetProductsAsync()
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    // SQL query to select product details, joining with 'medidas' table to get measurement descriptions
                    string consulta = @"
                        SELECT 
                            p.codigo, 
                            p.nombre, 
                            p.descripcion, 
                            p.precio, 
                            p.existencia, 
                            p.usa_stock,
                            m.desc_medidas 
                        FROM productos p
                        JOIN medidas m ON p.id_medidas = m.id_medidas";

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        using (DbDataReader lector = await comando.ExecuteReaderAsync())
                        {
                            while (await lector.ReadAsync())
                            {
                                Producto producto = new Producto
                                {
                                    Codigo = lector.IsDBNull(lector.GetOrdinal("codigo")) ? 0 : lector.GetInt32("codigo"),
                                    Nombre = lector.IsDBNull(lector.GetOrdinal("nombre")) ? "N/A" : lector.GetString("nombre"),
                                    Descripcion = lector.IsDBNull(lector.GetOrdinal("descripcion")) ? "N/A" : lector.GetString("descripcion"),
                                    Precio = lector.IsDBNull(lector.GetOrdinal("precio")) ? 0.00m : lector.GetDecimal("precio"),
                                    Existencia = lector.IsDBNull(lector.GetOrdinal("existencia")) ? 0 : lector.GetInt32("existencia"),
                                    Medida = lector.IsDBNull(lector.GetOrdinal("desc_medidas")) ? "N/A" : lector.GetString("desc_medidas"),
                                    UsaStock = lector.IsDBNull(lector.GetOrdinal("usa_stock")) ? 0 : lector.GetInt32("usa_stock"),
                                    Cantidad = 0
                                };

                                productos.Add(producto);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los productos: " + ex.Message);
            }

            return productos;
        }

        /// Asynchronously updates the stock quantities of products in the database after a sale.
        /// <param name="productos">A list of products with quantities to be deducted from stock.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <param name="productos">A list of products with quantities to be deducted from stock.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task UpdateStockAsync(List<Producto> productos)
        {
            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    foreach (var producto in productos)
                    {
                        // Verificar si el producto usa control de inventario (UsaStock == 1)
                        if (producto.UsaStock == 1)
                        {
                            // Verificar si hay suficiente stock disponible
                            if (producto.Cantidad > producto.Existencia)
                            {
                                throw new Exception($"No hay suficiente stock para el producto con código {producto.Codigo}.");
                            }

                            // Consulta para actualizar la cantidad de productos
                            string consulta = @"UPDATE productos 
                                        SET existencia = existencia - @CantidadVendida 
                                        WHERE codigo = @Codigo";

                            using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                            {
                                // Agregar parámetros
                                comando.Parameters.AddWithValue("@CantidadVendida", producto.Cantidad);
                                comando.Parameters.AddWithValue("@Codigo", producto.Codigo);

                                // Ejecutar el comando
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

                        string consulta = @"SELECT p.codigo, p.nombre, p.descripcion, p.precio, p.existencia, m.desc_medidas 
                                        FROM productos p
                                        JOIN medidas m ON p.id_medidas = m.id_medidas
                                        WHERE p.existencia = 0";

                        using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                        {
                            using (DbDataReader lector = await comando.ExecuteReaderAsync())
                            {
                                while (await lector.ReadAsync())
                                {
                                    Producto producto = new Producto
                                    {
                                        Codigo = lector.IsDBNull(lector.GetOrdinal("codigo")) ? 0 : lector.GetInt32("codigo"),
                                        Nombre = lector.IsDBNull(lector.GetOrdinal("nombre")) ? "N/A" : lector.GetString("nombre"),
                                        Descripcion = lector.IsDBNull(lector.GetOrdinal("descripcion")) ? "N/A" : lector.GetString("descripcion"),
                                        Precio = lector.IsDBNull(lector.GetOrdinal("precio")) ? 0.00m : lector.GetDecimal("precio"),
                                        Existencia = lector.IsDBNull(lector.GetOrdinal("existencia")) ? 0 : lector.GetInt32("existencia"),
                                        Medida = lector.IsDBNull(lector.GetOrdinal("desc_medidas")) ? "N/A" : lector.GetString("desc_medidas"),
                                        Cantidad = 0
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
    }
}




