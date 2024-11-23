using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Database
{
    public class Department
    {
        public int Id { get; set; } // id_departamento
        public required string Nombre { get; set; } // Nm_Dept
        public required string Descripcion { get; set; } // Descripcion
    }

    public class DepartmentManager
    {
        /// Asynchronously retrieves a list of departments from the database.
        public async Task<List<Department>> GetAlltAsync()
        {
            List<Department> departamentos = new List<Department>();

            try
            {
                // Establish a connection to the database using the connection manager
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    // Open the database connection asynchronously
                    await conexion.OpenAsync();

                    // SQL query to retrieve department information
                    string consulta = @"SELECT id_departamento, Nm_Dept, Descripcion FROM departamentos";

                    // Execute the query using MySqlCommand and the established connection
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        // Execute the command asynchronously and return a data reader
                        using (DbDataReader lector = await comando.ExecuteReaderAsync())
                        {
                            // Read each row returned by the query
                            while (await lector.ReadAsync())
                            {
                                // Map each field in the row to the Department model's properties
                                Department? departamento = new Department
                                {
                                    Id = lector.IsDBNull(lector.GetOrdinal("id_departamento"))
                                         ? 0
                                         : lector.GetInt32("id_departamento"), // Handle NULL for int
                                    Nombre = lector.IsDBNull(lector.GetOrdinal("Nm_Dept"))
                                             ? "N/A"
                                             : lector.GetString("Nm_Dept"), // Handle NULL for string
                                    Descripcion = lector.IsDBNull(lector.GetOrdinal("Descripcion"))
                                                  ? "Sin Descripción"
                                                  : lector.GetString("Descripcion") // Handle NULL for optional string
                                };

                                // Add the mapped Department instance to the list of departments
                                departamentos.Add(departamento);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors encountered during database access
                throw new Exception("Error al obtener los departamentos: " + ex.Message);
            }

            // Return the list of departments
            return departamentos;
        }
    }
}
