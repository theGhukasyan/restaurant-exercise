using RestaurantExercise.Models;

namespace RestaurantExercise.Managers;

public interface IRestManager
{
    /// <summary>
    /// Handles the event when a new client group arrives at the restaurant.
    /// Attempts to seat the group at an available table; if successful, the group is seated, and a message is printed.
    /// If no suitable table is available, the group is added to the queue, and the thread waits for available tables.
    /// </summary>
    /// <param name="group">The client group that has arrived.</param>
    public void OnArrive(ClientsGroup group);
    
    /// <summary>
    /// Handles the event when a client group leaves, either after being served or abandoning the queue.
    /// If the group has been seated, the associated table is marked as available.
    /// If the group is still in the queue or has already left, the group is removed from the queue.
    /// </summary>
    /// <param name="group">The client group that is leaving.</param>
    public void OnLeave(ClientsGroup group);

    /// <summary>
    /// Looks up and returns a suitable table for seating a client group based on the group's size.
    /// The method searches through the available tables, considering both size compatibility
    /// and a permissible difference in size, and returns the first matching table.
    /// If no suitable table is found, it returns null.
    /// </summary>
    /// <param name="group">The client group for which to find a suitable table.</param>
    /// <returns>
    /// A suitable table if found; otherwise, null if no matching table is available.
    /// </returns>
    public Table? Lookup(ClientsGroup group);

    /// <summary>
    /// Gets the list of tables in the restaurant.
    /// </summary>
    /// <returns>A copy of the list of tables to avoid external modification.</returns>
    public List<Table> GetTables();

    /// <summary>
    /// Checks if a specific table is currently occupied by a client group.
    /// </summary>
    /// <param name="table">The table to check.</param>
    /// <returns>True if the table is currently occupied, otherwise false.</returns>
    public bool IsTableOccupied(Table table);

    /// <summary>
    /// Gets the queue of client groups waiting for a table.
    /// </summary>
    /// <returns>A copy of the queue to avoid external modification.</returns>
    public Queue<ClientsGroup> GetQueue();
}