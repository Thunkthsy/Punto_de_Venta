using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Models;
using System.Data;

namespace Database
{
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
                                    Codigo = lector.GetInt32("codigo"),
                                    Nombre = lector.GetString("nombre"),
                                    Descripcion = lector.GetString("descripcion"),
                                    Precio = lector.GetDecimal("precio"),
                                    Existencia = lector.GetInt32("existencia"),
                                    Medida = lector.GetString("desc_medidas"),
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

