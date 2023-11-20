using RestaurantExercise.Managers;
using RestaurantExercise.Models;

namespace RestaurantExercise.Tests;

public class RestManagerTests
{
    [Fact]
    public void OnArrive_SeatsGroupWhenTableIsAvailable()
    {
        // Arrange
        var tables = new List<Table> { new Table(4) };
        var restManager = new RestManager(tables);

        // Act
        restManager.OnArrive(new ClientsGroup(3));

        // Assert
        Assert.Single(restManager.GetTables());
    }

    [Fact]
    public void OnArrive_AddsToQueueWhenNoTableAvailable()
    {
        // Arrange
        var tables = new List<Table> { new Table(2) };
        var restManager = new RestManager(tables);

        // Act
        restManager.OnArrive(new ClientsGroup(4));

        // Assert
        Assert.Single(restManager.GetQueue());
    }

    [Fact]
    public void OnLeave_RemovesGroupFromQueueWhenSeated()
    {
        // Arrange
        var tables = new List<Table> { new Table(4) };
        var restManager = new RestManager(tables);
        var group = new ClientsGroup(3);
        restManager.OnArrive(group);

        // Act
        restManager.OnLeave(group);

        // Assert
        Assert.Empty(restManager.GetQueue());
    }
    
    [Fact]
    public void Lookup_ReturnsTableWhenAvailable()
    {
        // Arrange
        var tables = new List<Table> { new Table(5) };
        var restManager = new RestManager(tables);
        var group = new ClientsGroup(4);
        restManager.OnArrive(group);

        // Act
        var result = restManager.Lookup(group);

        // Assert
        Assert.NotNull(result);
    }
}