# Restaurant Manager
The <b>Restaurant Manager</b> is a class that manages the seating and leaving of client groups in a restaurant. It ensures proper synchronization and coordination between arriving groups, leaving groups, and available tables.

## Features
- <b>OnArrive(ClientsGroup group)</b>: Handles the arrival of client groups, attempting to seat them at available tables. If no suitable table is available, the group is added to a waiting queue.
- <b>OnLeave(ClientsGroup group)</b>: Manages the leaving of client groups. If a group was seated, the table is marked as vacant, and waiting groups are notified. If the group was in the waiting queue, it is removed from the queue.
- <b>Lookup(ClientsGroup group)</b>: Checks for an available table suitable for the given client group based on size. Returns a copy of the table if available, or null otherwise.
- <b>GetTables()</b>: Retrieves a list of currently occupied tables.
- <b>IsTableOccupied(Table table)</b>: Checks if a specific table is currently occupied.
- <b>GetQueue()</b>: Retrieves the waiting queue of client groups.

## Thread Safety
The class ensures thread safety through the use of locks and monitors to synchronize access to critical sections of code, preventing race conditions and ensuring proper coordination between threads.

## Unit Tests
The unit tests cover various scenarios, including concurrent operations and edge cases, ensuring the reliability and correctness of the '<b>RestManager</b>' functionalities.

## Usage
```csharp
// Example Usage

// Initialize the Restaurant Manager with a list of tables
var tables = new List<Table> { new Table(4), new Table(2) };
var restManager = new RestManager(tables);

// Client group arrives
var group1 = new ClientsGroup(3);
restManager.OnArrive(group1);

// Client group leaves
var group2 = new ClientsGroup(2);
restManager.OnLeave(group2);
```
