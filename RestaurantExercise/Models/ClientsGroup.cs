namespace RestaurantExercise.Models;

public class ClientsGroup
{
    public int Size { get; set; }
    
    public ClientsGroup(int size) => Size = size;
    
    public override bool Equals(object? obj)
    {
        if (obj is ClientsGroup other)
        {
            return this.Size == other.Size;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Size.GetHashCode();
    }
}