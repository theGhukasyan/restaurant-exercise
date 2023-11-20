using RestaurantExercise.Models;

namespace RestaurantExercise.Managers;

public class RestManager : IRestManager
{
    private readonly List<Table> _tables;
    private Queue<ClientsGroup> queue;

    private readonly object _lockObject = new ();

    public RestManager(List<Table> tables)
    {
        _tables = tables ?? throw new ArgumentNullException(nameof(tables));
        queue = new Queue<ClientsGroup>();
    }

    /// <inheritdoc />
    public void OnArrive(ClientsGroup group)
    {
        lock (_lockObject)
        {
            if (!TrySeatGroup(group))
            {
                // Enqueue a copy to avoid external modification
                queue.Enqueue(new ClientsGroup(group.Size)); 
            
                // Notify waiting threads
                Monitor.PulseAll(_lockObject);
            }
        }
    }

    /// <inheritdoc />
    public void OnLeave(ClientsGroup group)
    {
        lock (_lockObject)
        {
            var table = Lookup(group);
            if (table != null)
            {
                _tables.Remove(table);

                // Notify waiting threads
                Monitor.PulseAll(_lockObject);
            }
            else
            {
                queue = new Queue<ClientsGroup>(queue.Where(g => g != group));

                // Notify waiting threads
                Monitor.PulseAll(_lockObject);
            }
        }
    }
    
    /// <inheritdoc />
    public Table? Lookup(ClientsGroup group)
    {
        lock (_lockObject)
        {
            var availableTable = _tables.FirstOrDefault(table => IsTableSizeValid(table, group) && IsDifferenceValid(table, group));
            return availableTable != null ? new Table(availableTable.Size) : null;
        }
    }
    
    public List<Table> GetTables()
    {
        lock (_lockObject)
        {
            return new List<Table>(_tables);
        }
    }

    public bool IsTableOccupied(Table table)
    {
        lock (_lockObject)
        {
            return _tables.Contains(table);
        }
    }

    public Queue<ClientsGroup> GetQueue()
    {
        lock (_lockObject)
        {
            return new Queue<ClientsGroup>(queue);
        }
    }
    
    /// <summary>
    /// Attempts to seat a client group at an available table by looking up a suitable table based on the group size.
    /// If a suitable table is found, the group is seated at that table, and the table is marked as occupied.
    /// </summary>
    /// <param name="group">The client group to seat.</param>
    /// <returns>True if the group is successfully seated, otherwise false if no suitable table is available.</returns>
    private bool TrySeatGroup(ClientsGroup group)
    {
        var availableTable = Lookup(group);

        if (availableTable != null)
        {
            _tables.Remove(availableTable);
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// Checks whether the size of a given table is sufficient to accommodate the client group.
    /// </summary>
    /// <param name="table">The table to check.</param>
    /// <param name="group">The client group for which to check table size.</param>
    /// <returns>True if the table size is greater than or equal to the client group size, otherwise false.</returns>
    private static bool IsTableSizeValid(Table table, ClientsGroup group) 
        => table.Size >= group.Size;

    /// <summary>
    /// Checks whether the difference between the size of a given table and the size of the client group is within an acceptable range.
    /// This provides a tolerance for the difference in size between the table and the client group,
    /// allowing the group to be seated at a slightly larger table if needed.
    /// </summary>
    /// <param name="table">The table to check.</param>
    /// <param name="group">The client group for which to check the size difference.</param>
    /// <returns>True if the size difference is less than 2, otherwise false.</returns>
    private static bool IsDifferenceValid(Table table, ClientsGroup group) 
        => table.Size - group.Size < 2;
}