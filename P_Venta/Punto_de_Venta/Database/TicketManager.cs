using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Models;
using System.Data;

namespace Database
{
    // TicketManager class handles database operations related to tickets
    public class TicketManager
    {
        /// <summary>
        /// Retrieves a list of available ticket folios (numbers) that are not currently active.
        /// </summary>
        /// <returns>A list of available ticket folios.</returns>
        public static async Task<List<int>> GetAvailableFoliosAsync()
        {
            List<int> availableFolios = new List<int>();
            int maxTickets = 5; // Maximum number of tickets allowed

            try
            {
                using (var connection = DatabaseConnectionManager.GetConnection())
                {
                    await connection.OpenAsync();

                    // Query to get all active ticket folios (estado = 1 means active)
                    string query = "SELECT ticket_folio FROM tickets WHERE estado = 1";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        HashSet<int> activeFolios = new HashSet<int>();

                        // Execute the query and read the results
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int folio = reader.GetInt32("ticket_folio");
                                activeFolios.Add(folio);
                            }
                        }

                        // Determine available folios up to the maxTickets limit
                        for (int i = 1; i <= maxTickets; i++)
                        {
                            if (!activeFolios.Contains(i))
                            {
                                availableFolios.Add(i);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log the error)
                Console.WriteLine($"Error fetching ticket folios: {ex.Message}");
            }

            return availableFolios;
        }

        /// <summary>
        /// Retrieves ticket details by folio (ticket number).
        /// </summary>
        /// <param name="folio">The folio number of the ticket.</param>
        /// <returns>A Ticket object with details and associated products.</returns>
        public static async Task<Ticket> GetTicketDetailsAsync(int folio)
        {
            Ticket? ticket = null;

            try
            {
                using (var connection = DatabaseConnectionManager.GetConnection())
                {
                    await connection.OpenAsync();

                    // Query to get ticket details by folio
                    string query = "SELECT id_ticket, ticket_folio, estado, total_ticket FROM tickets WHERE ticket_folio = @folio";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@folio", folio);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                ticket = new Ticket
                                {
                                    IdTicket = reader.GetInt32("id_ticket"),
                                    Folio = reader.GetInt32("ticket_folio"),
                                    Estado = reader.GetInt32("estado"),
                                    TotalTicket = reader.GetDecimal("total_ticket")
                                };
                            }
                        }
                    }

                    if (ticket != null)
                    {
                        // Get the list of products associated with the ticket
                        ticket.Productos = await GetProductosByTicketFolioAsync(folio);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                Console.WriteLine($"Error fetching ticket details: {ex.Message}");
            }

            return ticket;
        }

        /// <summary>
        /// Retrieves the list of products associated with a specific ticket folio.
        /// </summary>
        /// <param name="folio">The folio number of the ticket.</param>
        /// <returns>An ObservableCollection of Producto objects.</returns>
        private static async Task<ObservableCollection<Producto>> GetProductosByTicketFolioAsync(int folio)
        {
            var productos = new ObservableCollection<Producto>();

            try
            {
                using (var connection = DatabaseConnectionManager.GetConnection())
                {
                    await connection.OpenAsync();

                    // Query to get products sold associated with the ticket folio
                    string query = @"
                        SELECT pv_code, pv_nombre, pv_precio, pv_cantidad 
                        FROM productos_vendidos 
                        WHERE ticket_folio = @folio";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@folio", folio);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var producto = new Producto
                                {
                                    Codigo = reader.GetInt32("pv_code"),
                                    Nombre = reader.GetString("pv_nombre"),
                                    Precio = reader.GetDecimal("pv_precio"),
                                    Cantidad = reader.GetInt32("pv_cantidad"),
                                    // Assuming Descripcion and Medida are optional or can be set to default values
                                    Descripcion = string.Empty,
                                    Medida = string.Empty,
                                    // Add other properties as needed
                                };
                                productos.Add(producto);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                Console.WriteLine($"Error fetching products: {ex.Message}");
            }

            return productos;
        }
    }
}
