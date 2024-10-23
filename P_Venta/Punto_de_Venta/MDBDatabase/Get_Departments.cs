using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Database
{
    public class Get_Departments
    {
        public List<string> ObtenerNombresDepartamentos()
        {
            List<string> nombresDepartamentos = new List<string>();

            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    conexion.Open();

                    string consulta = "SELECT Nombre FROM Departamentos"; // Ajusta según tu base de datos

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        using (MySqlDataReader lector = comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                string nombre = lector["Nombre"].ToString();
                                nombresDepartamentos.Add(nombre);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los nombres de los departamentos: " + ex.Message);
            }

            return nombresDepartamentos;
        }
    }
}
