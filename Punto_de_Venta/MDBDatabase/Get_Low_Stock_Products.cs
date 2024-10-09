using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
namespace Database
{
    public class Get_Low_Stock_Products
    {
        public List<App.Producto> ObtenerProductosConStockCero()
        {
            List<App.Producto> productos = new List<App.Producto>();

            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    conexion.Open();

                    // Query to get products with zero stock
                    string consulta = @"SELECT p.codigo, p.nombre, p.descripcion, p.precio, p.existencia, m.desc_medidas 
                                        FROM productos p
                                        JOIN medidas m ON p.id_medidas = m.id_medidas
                                        WHERE p.existencia = 0";

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        using (MySqlDataReader lector = comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                // Populate Producto object from data
                                App.Producto producto = new App.Producto
                                {
                                    Codigo = lector.IsDBNull(lector.GetOrdinal("codigo")) ? 0 : lector.GetInt32("codigo"),
                                    Nombre = lector.IsDBNull(lector.GetOrdinal("nombre")) ? "N/A" : lector.GetString("nombre"),
                                    Descripcion = lector.IsDBNull(lector.GetOrdinal("descripcion")) ? "N/A" : lector.GetString("descripcion"),
                                    Precio = lector.IsDBNull(lector.GetOrdinal("precio")) ? 0.00m : lector.GetDecimal("precio"),
                                    Existencia = lector.IsDBNull(lector.GetOrdinal("existencia")) ? 0 : lector.GetInt32("existencia"),
                                    Medida = lector.IsDBNull(lector.GetOrdinal("desc_medidas")) ? "N/A" : lector.GetString("desc_medidas")
                                };

                                productos.Add(producto);
                            }
                        }
                    }
                }

                // If no products with zero stock are found, throw an exception with the message
                if (productos.Count == 0)
                {
                    throw new Exception("Ningún producto sin stock.");
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
