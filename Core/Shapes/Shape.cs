namespace Core.Shapes;

public class BaseShape
{
    public int[,] Blocks { get; protected set; } = new int[0, 0];
    public int[,] RotatedBlocks { get; protected set; } = new int[0, 0];
    public int X { get; set; }
    public int Y { get; set; }
}

public class Shape : BaseShape, IMovable, IRotatable
{
    public void MoveDown()
    {
        Y++;
    }

    public void MoveLeft()
    {
        X--;
    }

    public void MoveRight()
    {
        X++;
    }

    public void Rotate()
    {
        var rows = Blocks.GetLength(0);
        var cols = Blocks.GetLength(1);
        var rotated = new int[cols, rows];
        for (var y = 0; y < rows; y++)
        {
            for (var x = 0; x < cols; x++)
            {
                rotated[x, rows - 1 - y] = Blocks[y, x];
            }
        }
        
        Blocks = rotated;
    }

    public Shape Copy()
    {
        var shape = (Shape)MemberwiseClone();
        shape.Blocks = (int[,])Blocks.Clone();
        return shape;
    }
    
};