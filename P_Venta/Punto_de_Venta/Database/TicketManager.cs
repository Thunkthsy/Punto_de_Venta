using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Models;
using System.Data;

namespace Database
{
    public class TicketManager
    {
        // Asynchronously checks whether the folio exists in the database. 
        // This method has been replaced by `if (TicketFolios.Contains(Selected_Folio))` 
        // in the main window's ticket-saving methods.
        public static async Task<bool> CheckFolioExistsAsync(int SelectedFolio)
        {
            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    string query = "SELECT COUNT(*) FROM tickets WHERE ticket_folio = @folio";

                    using (MySqlCommand command = new MySqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@folio", SelectedFolio);

                        var result = await command.ExecuteScalarAsync();
                        return Convert.ToInt32(result) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar el folio: {ex.Message}");
                throw new Exception("Se produjo un error al verificar el folio.", ex);
            }
        }

        // Retrieves a ticket by its folio number.
        public static async Task<Ticket?> GetTicketByFolioAsync(int folio)
        {
            Ticket? ticket = null;

            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    string consulta = @"
                            SELECT 

                                t.ticket_folio,
                                t.estado,
                                t.total_ticket,
                                pv.id_vendido,
                                pv.pv_code AS codigo,
                                pv.pv_nombre AS nombre,
                                pv.pv_precio AS precio,
                                pv.cantidad,
                                d.Nm_Dept AS departamento
                            FROM 
                                tickets t
                            JOIN 
                                p_vendidos pv ON t.ticket_folio = pv.ticket_folio
                            LEFT JOIN 
                                departamentos d ON pv.pv_dept_id = d.id_departamento
                            WHERE 
                                t.ticket_folio = @folio";

                    using (MySqlCommand command = new MySqlCommand(consulta, conexion))
                    {
                        command.Parameters.AddWithValue("@folio", folio);

                        using (DbDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                if (ticket == null)
                                {
                                    ticket = new Ticket
                                    {
                                        Folio = reader.GetInt32("ticket_folio"),
                                        Estado = reader.GetInt32("estado"),
                                        TotalTicket = reader.GetDecimal("total_ticket"),
                                        Productos = new ObservableCollection<Producto>()
                                    };
                                }

                                int codigo = reader.GetInt32("codigo");
                                Producto producto = new Producto
                                {
                                    Codigo = codigo,
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? "N/A" : reader.GetString("nombre"),
                                    Precio = reader.IsDBNull(reader.GetOrdinal("precio")) ? 0.00m : reader.GetDecimal("precio"),
                                    Cantidad = reader.GetInt32("cantidad"),
                                    Departamento = reader.IsDBNull(reader.GetOrdinal("departamento")) ? "N/A" : reader.GetString("departamento"),
                                    Descripcion = "N/A",
                                    Medida = "N/A",
                                };

                                ticket.Productos.Add(producto);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo un error al recuperar el ticket por folio: {ex.Message}");
                throw new Exception("Error al recuperar el ticket por folio.", ex);
            }

            return ticket;
        }

        // Retrieves a list of open ticket folios (estado = 1).
        public static async Task<List<int>> GetOpenFoliosAsync()
        {
            List<int> folios = new List<int>();

            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    string consulta = "SELECT ticket_folio FROM tickets WHERE estado = 1;";

                    using (MySqlCommand command = new MySqlCommand(consulta, conexion))
                    {
                        using (DbDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                folios.Add(reader.GetInt32("ticket_folio"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recuperar los folios de tickets abiertos: {ex.Message}");
                throw new Exception("Se produjo un error al recuperar los folios de tickets abiertos.", ex);
            }

            return folios;
        }

        // Gets the next available ticket folio, incrementing from the highest existing folio on database.
        public static async Task<int> GetNextFolioAsync()
        {
            int nextFolio = 1; // Default starting folio if no records exist.

            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();
                    // Query to fetch the highest ticket folio.
                    string consulta = "SELECT MAX(ticket_folio) FROM tickets WHERE estado IN (1, 2);";
                    using (MySqlCommand command = new MySqlCommand(consulta, conexion))
                    {
                        var result = await command.ExecuteScalarAsync();
                        if (result != DBNull.Value)
                        {   // Increment the highest fetched folio by 1 to determine the next available ticket folio.
                            nextFolio = Convert.ToInt32(result) + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el siguiente folio: {ex.Message}");
                throw new Exception("Se produjo un error al recuperar el siguiente folio de ticket.", ex);
            }

            return nextFolio;
        }

        // Creates a new ticket by inserting a ticket with a new folio and default values.
        public static async Task<int> CreateNewTicketAsync()
        {
            try
            {
                int newFolio = await GetNextFolioAsync();

                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    string insertQuery = "INSERT INTO tickets (ticket_folio, estado, total_ticket, ticket_fecha) VALUES (@folio, 1, 0, @ticket_fecha)";
                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, conexion))
                    {
                        insertCommand.Parameters.AddWithValue("@folio", newFolio);
                        insertCommand.Parameters.AddWithValue("@ticket_fecha", DateTime.Now);

                        await insertCommand.ExecuteNonQueryAsync();
                    }
                }

                return newFolio;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el ticket: {ex.Message}");
                throw new Exception("Se produjo un error al crear el ticket.", ex);
            }
        }

        /// Saves a ticket and its associated products to the database.
        public static async Task SaveTicketAsync(Ticket ticket)
        {
            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    // Start a transaction to ensure atomicity of ticket and product inserts
                    using (MySqlTransaction transaction = await conexion.BeginTransactionAsync())
                    {
                        try
                        {
                            // Insert the ticket details into the tickets table
                            string insertTicketQuery = @"
                            INSERT INTO tickets (ticket_folio, estado, total_ticket, ticket_fecha) 
                            VALUES (@folio, @estado, @total, @fecha)";

                            using (MySqlCommand ticketCommand = new MySqlCommand(insertTicketQuery, conexion, transaction))
                            {
                                ticketCommand.Parameters.AddWithValue("@folio", ticket.Folio);
                                ticketCommand.Parameters.AddWithValue("@estado", ticket.Estado);
                                ticketCommand.Parameters.AddWithValue("@total", ticket.TotalTicket);
                                ticketCommand.Parameters.AddWithValue("@fecha", ticket.TicketFecha);

                                await ticketCommand.ExecuteNonQueryAsync();
                            }

                            // Insert each product in the Productos collection into the p_vendidos table
                            string insertProductQuery = @"
                            INSERT INTO p_vendidos (ticket_folio, pv_code, pv_nombre, pv_precio, cantidad, pv_dept_id) 
                            VALUES (@folio, @codigo, @nombre, @precio, @cantidad, @pv_dept_id)";

                            foreach (var producto in ticket.Productos)
                            {
                                // Query the departamentos table to get the id_departamento for Nm_Dept
                                string getDepartmentIdQuery = "SELECT id_departamento FROM departamentos WHERE Nm_Dept = @nmDept";
                                int departamentoId;

                                using (MySqlCommand deptCommand = new MySqlCommand(getDepartmentIdQuery, conexion, transaction))
                                {
                                    deptCommand.Parameters.AddWithValue("@nmDept", producto.Departamento);
                                    var result = await deptCommand.ExecuteScalarAsync();

                                    if (result == null)
                                    {
                                        throw new Exception($"No se encontró un departamento que coincida con {producto.Departamento}");
                                    }

                                    departamentoId = Convert.ToInt32(result);
                                }

                                // Insert the product with the obtained departamentoId
                                using (MySqlCommand productCommand = new MySqlCommand(insertProductQuery, conexion, transaction))
                                {
                                    productCommand.Parameters.AddWithValue("@folio", ticket.Folio);
                                    productCommand.Parameters.AddWithValue("@codigo", producto.Codigo);
                                    productCommand.Parameters.AddWithValue("@nombre", producto.Nombre);
                                    productCommand.Parameters.AddWithValue("@precio", producto.Precio);
                                    productCommand.Parameters.AddWithValue("@cantidad", producto.Cantidad);
                                    productCommand.Parameters.AddWithValue("@pv_dept_id", departamentoId);

                                    await productCommand.ExecuteNonQueryAsync();
                                }
                            }

                            // Commit the transaction if all commands succeed
                            await transaction.CommitAsync();
                        }
                        catch (Exception)
                        {
                            // Rollback the transaction if any command fails
                            await transaction.RollbackAsync();
                            throw; // Re-throw the exception to be handled by the caller
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el ticket: {ex.Message}");
                throw new Exception("Se produjo un error al guardar el ticket.", ex);
            }
        }

        // Retrieves a list of closed ticket folios (estado = 2) based on a specific date.
        public static async Task<List<int>> GetClosedFoliosByDateAsync(DateTime date)
        {
            List<int> closedFolios = new List<int>();

            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    // Query to select closed folios on the specified date
                    string consulta = @"
                SELECT ticket_folio 
                FROM tickets 
                WHERE estado = 2 AND DATE(ticket_fecha) = @date";

                    using (MySqlCommand command = new MySqlCommand(consulta, conexion))
                    {
                        // Set the date parameter
                        command.Parameters.AddWithValue("@date", date.Date);

                        using (DbDataReader reader = await command.ExecuteReaderAsync())
                        {
                            // Add each closed folio to the list
                            while (await reader.ReadAsync())
                            {
                                closedFolios.Add(reader.GetInt32("ticket_folio"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recuperar los folios de tickets cerrados: {ex.Message}");
                throw new Exception("Se produjo un error al recuperar los folios de tickets cerrados.", ex);
            }

            return closedFolios;
        }

        // Updates an existing ticket in the database.
        // If a ticket with the specified folio exists, it and its associated products are deleted,
        // then replaced with the updated ticket data provided in the ticket object.
        // Ensures the database reflects the most current ticket and related product information.
        public static async Task UpdateTicketAsync(Ticket ticket)
        {
            try
            {
                using (MySqlConnection conexion = DatabaseConnectionManager.GetConnection())
                {
                    await conexion.OpenAsync();

                    // Start a transaction to ensure atomicity of delete and insert operations
                    using (MySqlTransaction transaction = await conexion.BeginTransactionAsync())
                    {
                        try
                        {
                            // Delete the existing ticket and its associated products
                            string deleteTicketQuery = "DELETE FROM tickets WHERE ticket_folio = @folio";
                            string deleteProductsQuery = "DELETE FROM p_vendidos WHERE ticket_folio = @folio";

                            using (MySqlCommand deleteTicketCommand = new MySqlCommand(deleteTicketQuery, conexion, transaction))
                            {
                                deleteTicketCommand.Parameters.AddWithValue("@folio", ticket.Folio);
                                await deleteTicketCommand.ExecuteNonQueryAsync();
                            }

                            using (MySqlCommand deleteProductsCommand = new MySqlCommand(deleteProductsQuery, conexion, transaction))
                            {
                                deleteProductsCommand.Parameters.AddWithValue("@folio", ticket.Folio);
                                await deleteProductsCommand.ExecuteNonQueryAsync();
                            }

                            // Insert the updated ticket details into the tickets table
                            string insertTicketQuery = @"
                            INSERT INTO tickets (ticket_folio, estado, total_ticket, ticket_fecha) 
                            VALUES (@folio, @estado, @total, @fecha)";

                            using (MySqlCommand ticketCommand = new MySqlCommand(insertTicketQuery, conexion, transaction))
                            {
                                ticketCommand.Parameters.AddWithValue("@folio", ticket.Folio);
                                ticketCommand.Parameters.AddWithValue("@estado", ticket.Estado);
                                ticketCommand.Parameters.AddWithValue("@total", ticket.TotalTicket);
                                ticketCommand.Parameters.AddWithValue("@fecha", ticket.TicketFecha);

                                await ticketCommand.ExecuteNonQueryAsync();
                            }

                            // Insert each product in the Productos collection into the p_vendidos table
                            string insertProductQuery = @"
                            INSERT INTO p_vendidos (ticket_folio, pv_code, pv_nombre, pv_precio, cantidad, pv_dept_id) 
                            VALUES (@folio, @codigo, @nombre, @precio, @cantidad, @pv_dept_id)";

                            foreach (var producto in ticket.Productos)
                            {
                                // Query the departamentos table to get the id_departamento for Nm_Dept
                                string getDepartmentIdQuery = "SELECT id_departamento FROM departamentos WHERE Nm_Dept = @nmDept";
                                int departamentoId;

                                using (MySqlCommand deptCommand = new MySqlCommand(getDepartmentIdQuery, conexion, transaction))
                                {
                                    deptCommand.Parameters.AddWithValue("@nmDept", producto.Departamento);
                                    var result = await deptCommand.ExecuteScalarAsync();

                                    if (result == null)
                                    {
                                        throw new Exception($"No se encontró un departamento que coincida con {producto.Departamento}");
                                    }

                                    departamentoId = Convert.ToInt32(result);
                                }

                                // Insert the product with the obtained departamentoId
                                using (MySqlCommand productCommand = new MySqlCommand(insertProductQuery, conexion, transaction))
                                {
                                    productCommand.Parameters.AddWithValue("@folio", ticket.Folio);
                                    productCommand.Parameters.AddWithValue("@codigo", producto.Codigo);
                                    productCommand.Parameters.AddWithValue("@nombre", producto.Nombre);
                                    productCommand.Parameters.AddWithValue("@precio", producto.Precio);
                                    productCommand.Parameters.AddWithValue("@cantidad", producto.Cantidad);
                                    productCommand.Parameters.AddWithValue("@pv_dept_id", departamentoId);

                                    await productCommand.ExecuteNonQueryAsync();
                                }
                            }

                            // Commit the transaction if all commands succeed
                            await transaction.CommitAsync();
                        }
                        catch (Exception)
                        {
                            // Rollback the transaction if any command fails
                            await transaction.RollbackAsync();
                            throw; // Re-throw the exception to be handled by the caller
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el ticket: {ex.Message}");
                throw new Exception("Se produjo un error al actualizar el ticket.", ex);
            }
        }
    }
}




