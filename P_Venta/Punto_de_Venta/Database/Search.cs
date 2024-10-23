using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Models;
using System.Data;

namespace Database
{
    public class Search
    {
        
        public static async Task<List<Producto>> ProductsAsync(string searchText)
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    string consulta = @"SELECT p.codigo, p.nombre, p.descripcion, p.precio, p.existencia, p.usa_stock, m.desc_medidas 
                                        FROM productos p
                                        JOIN medidas m ON p.id_medidas = m.id_medidas
                                        WHERE LOWER(p.nombre) LIKE LOWER(@searchText) OR p.codigo = @codigo
                                        ORDER BY CASE
                                            WHEN LOWER(p.nombre) LIKE LOWER(@searchText + '%') THEN 1
                                            WHEN LOWER(p.nombre) LIKE LOWER('%' + @searchText + '%') THEN 2
                                            ELSE 3
                                        END";

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                        if (int.TryParse(searchText, out int codigo))
                        {
                            comando.Parameters.AddWithValue("@codigo", codigo);
                        }
                        else
                        {
                            comando.Parameters.AddWithValue("@codigo", -1);
                        }

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
                                    UsaStock = lector.IsDBNull(lector.GetOrdinal("usa_stock")) ? 0 : lector.GetInt32("usa_stock"),
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
                throw new Exception("Error al buscar los productos: " + ex.Message);
            }

            return productos;
        }

        public static async Task<Producto?> ProductByCodeAsync(int codigo)
        {
            Producto? producto = null;

            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    string consulta = @"SELECT p.codigo, p.nombre, p.descripcion, p.precio, p.existencia, p.usa_stock, m.desc_medidas 
                                        FROM productos p
                                        JOIN medidas m ON p.id_medidas = m.id_medidas
                                        WHERE p.codigo = @codigo";

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@codigo", codigo);

                        using (DbDataReader lector = await comando.ExecuteReaderAsync())
                        {
                            if (await lector.ReadAsync())
                            {
                                producto = new Producto
                                {
                                    Codigo = lector.IsDBNull(lector.GetOrdinal("codigo")) ? 0 : lector.GetInt32("codigo"),
                                    Nombre = lector.IsDBNull(lector.GetOrdinal("nombre")) ? "N/A" : lector.GetString("nombre"),
                                    Descripcion = lector.IsDBNull(lector.GetOrdinal("descripcion")) ? "N/A" : lector.GetString("descripcion"),
                                    Precio = lector.IsDBNull(lector.GetOrdinal("precio")) ? 0.00m : lector.GetDecimal("precio"),
                                    Existencia = lector.IsDBNull(lector.GetOrdinal("existencia")) ? 0 : lector.GetInt32("existencia"),
                                    UsaStock = lector.IsDBNull(lector.GetOrdinal("usa_stock")) ? 0 : lector.GetInt32("usa_stock"),
                                    Medida = lector.IsDBNull(lector.GetOrdinal("desc_medidas")) ? "N/A" : lector.GetString("desc_medidas"),
                                    Cantidad = 0
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el producto por código: " + ex.Message);
            }

            return producto;
        }

        public async Task<List<Producto>> ObtenerProductosPorDepartamentoAsync(string nombreDepartamento)
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    string consulta = @"SELECT p.codigo, p.nombre, p.descripcion, p.precio, p.existencia, p.usa_stock, m.desc_medidas, d.Nombre AS departamento
                                        FROM productos p
                                        JOIN medidas m ON p.id_medidas = m.id_medidas
                                        JOIN departamentos d ON p.id_departamento = d.id_departamento
                                        WHERE d.Nombre = @nombreDepartamento";

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@nombreDepartamento", nombreDepartamento);

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
                                    UsaStock = lector.IsDBNull(lector.GetOrdinal("usa_stock")) ? 0 : lector.GetInt32("usa_stock"),
                                    Medida = lector.IsDBNull(lector.GetOrdinal("desc_medidas")) ? "N/A" : lector.GetString("desc_medidas"),
                                    Cantidad = 0,
                                    Departamento = lector.IsDBNull(lector.GetOrdinal("departamento")) ? "N/A" : lector.GetString("departamento")
                                };

                                productos.Add(producto);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los productos por departamento: " + ex.Message);
            }

            return productos;
        }
    }
}
