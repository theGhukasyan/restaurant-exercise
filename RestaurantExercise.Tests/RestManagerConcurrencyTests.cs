using RestaurantExercise.Managers;
using RestaurantExercise.Models;

namespace RestaurantExercise.Tests;

public class RestManagerConcurrencyTests
{
    [Fact]
    public void ConcurrentArrivalAndLeaving()
    {
        // Arrange
        var tables = new List<Table> { new Table(4), new Table(2) };
        var restManager = new RestManager(tables);

        // Simulate concurrent arrivals and leaving
        Parallel.ForEach(Enumerable.Range(1, 10), i =>
        {
            var group = new ClientsGroup(i % 3 + 2); // Vary group sizes (2, 3, 4, 2, 3, 4, ...)
            restManager.OnArrive(group);

            // Simulate some clients leaving after a while
            if (i % 2 == 0)
            {
                restManager.OnLeave(group);
            }
        });

        // Act
        var result = restManager.GetTables();

        // Assert
        Assert.All(result, table => Assert.True(table.Size is 0 or 2 or 4));
        Assert.True(restManager.GetQueue().Count == 0); // All groups should be served or leave
    }

    [Fact]
    public void ConcurrentArrivalAndLookup()
    {
        // Arrange
        var tables = new List<Table> { new Table(4), new Table(2) };
        var restManager = new RestManager(tables);

        // Simulate concurrent arrivals and lookups
        Parallel.ForEach(Enumerable.Range(1, 10), i =>
        {
            var group = new ClientsGroup(i % 3 + 2); // Vary group sizes (2, 3, 4, 2, 3, 4, ...)
            restManager.OnArrive(group);

            // Simulate some clients looking up tables after a while
            if (i % 2 == 0)
            {
                var table = restManager.Lookup(group);

                // Assertion: Ensure the table returned by Lookup is valid (null or with correct size)
                Assert.True(table == null || (table.Size == group.Size || table.Size - group.Size < 2));
            }
        });

        // Act
        var result = restManager.GetTables();

        // Assert
        Assert.All(result, table => Assert.True(table.Size is 0 or 2 or 4));
        Assert.True(restManager.GetQueue().Count == 0); // All groups should be served or leave
    }

    [Fact]
    public void ConcurrentLeavingAndLookup()
    {
        // Arrange
        var tables = new List<Table> { new Table(4), new Table(2) };
        var restManager = new RestManager(tables);

        // Simulate concurrent leaving and lookups
        Parallel.ForEach(Enumerable.Range(1, 10), i =>
        {
            var group = new ClientsGroup(i % 3 + 2); // Vary group sizes (2, 3, 4, 2, 3, 4, ...)
            restManager.OnArrive(group);

            // Simulate some clients leaving after a while
            if (i % 2 == 0)
            {
                restManager.OnLeave(group);
            }

            // Simulate some clients looking up tables after a while
            if (i % 3 == 0)
            {
                var table = restManager.Lookup(group);

                // Assertion: Ensure the table returned by Lookup is valid (null or with correct size)
                Assert.True(table == null || (table.Size == group.Size || table.Size - group.Size < 2));
            }
        });

        // Act
        var result = restManager.GetTables();

        // Assert
        Assert.All(result, table => Assert.True(table.Size is 0 or 2 or 4));
        Assert.True(restManager.GetQueue().Count == 0); // All groups should be served or leave
    }
    
    [Fact]
    public void ConcurrentArrivalLeavingAndLookup()
    {
        // Arrange
        var tables = new List<Table> { new Table(4), new Table(2) };
        var restManager = new RestManager(tables);

        // Simulate concurrent arrivals, leaving, and lookups
        Parallel.ForEach(Enumerable.Range(1, 10), i =>
        {
            var group = new ClientsGroup(i % 3 + 2); // Vary group sizes (2, 3, 4, 2, 3, 4, ...)

            // Simulate some clients looking up tables after a while
            if (i % 2 == 0)
            {
                restManager.OnArrive(group);
                var table = restManager.Lookup(group);

                // Assertion: Ensure the table returned by Lookup is valid (null or with correct size)
                Assert.True(table == null || (table.Size == group.Size || table.Size - group.Size < 2));
            }
            else
            {
                restManager.OnLeave(group);
            }
        });

        // Act
        var result = restManager.GetTables();

        // Assert
        Assert.All(result, table => Assert.True(table.Size is 0 or 2 or 4));
        Assert.True(restManager.GetQueue().Count == 0); // All groups should be served or leave
    }

    [Fact]
    public void ConcurrentArrivalAndMultipleLeaving()
    {
        // Arrange
        var tables = new List<Table> { new Table(4), new Table(2) };
        var restManager = new RestManager(tables);

        // Simulate concurrent arrivals and multiple clients leaving
        Parallel.ForEach(Enumerable.Range(1, 10), i =>
        {
            var group = new ClientsGroup(i % 3 + 2); // Vary group sizes (2, 3, 4, 2, 3, 4, ...)

            restManager.OnArrive(group);

            // Simulate multiple clients leaving after a while
            if (i % 3 == 0)
            {
                restManager.OnLeave(group);
            }
        });

        // Act
        var result = restManager.GetTables();

        // Assert
        Assert.All(result, table => Assert.True(table.Size is 0 or 2 or 4));
        Assert.True(restManager.GetQueue().Count == 0); // All groups should be served or leave
    }
}