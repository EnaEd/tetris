using Ardalis.GuardClauses;
using Core.Shapes;

namespace Core.Board;

public class Board
{
    public static readonly int Width = 10;
    public static readonly int Height = 20;

    private const int LineScore = 100;
    private List<Shape> Shapes { get; } = 
    [
        new IShape(),
        new JShape(),
        new LShape(),
        new OShape(),
        new SShape(),
        new TShape(),
        new ZShape()
    ];
    public int[,] Borders { get; } = new int[Height, Width];
    public Shape? CurrentShape { get; private set; }
    public int TotalScore { get; private set; }
    
    public void SpawnShape()
    {
        CurrentShape = Random.Shared.GetItems([..Shapes],1).First();
        
        CurrentShape.X = Width / 2 - CurrentShape.Blocks.GetLength(1) / 2;
        CurrentShape.Y = 0;
    }

    public bool CheckCollision(int nextX, int nextY)
    {
        Guard.Against.Null(CurrentShape);
        
        int rows = CurrentShape.Blocks.GetLength(0);
        int cols = CurrentShape.Blocks.GetLength(1);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (CurrentShape.Blocks[y, x] == 0)
                {
                    continue;
                }

                var borderX = nextX + x;
                var borderY = nextY + y;

                if (borderX < 0 || borderX >= Width || borderY < 0 || borderY >= Height)
                {
                    return true;
                }

                if (Borders[borderY, borderX] == 1)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    public bool LockAndSpawn()
    {
        Guard.Against.Null(CurrentShape);
        
        int rows = CurrentShape.Blocks.GetLength(0);
        int cols = CurrentShape.Blocks.GetLength(1);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
                if (CurrentShape.Blocks[y, x] == 1 && CurrentShape.Y + y >= 0)
                    Borders[CurrentShape.Y + y, CurrentShape.X + x] = 1;
        }
        
        int cleared = ClearLines();
        TotalScore += CalculateScore(cleared);
        SpawnShape();
        
        return !CheckCollision(CurrentShape.X, CurrentShape.Y);
    }

    public void RotateShape()
    {
        Guard.Against.Null(CurrentShape);
        var shape = CurrentShape.Copy();
        shape.Rotate();

        int[] kicks = [0, -1, 1, -2, 2];

        foreach (int offsetX in kicks)
        {
            if (!CheckCollision(shape.X + offsetX, shape.Y))
            {
                CurrentShape = shape;
                CurrentShape.X += offsetX;
                return; 
            }
        }
    }
    
    private static int CalculateScore(int cleared)
    {
        return cleared * LineScore;
    }
    
    private int ClearLines()
    {
        int lines = 0;
        for (int y = Height - 1; y >= 0; y--)
        {
            bool full = true;
            for (int x = 0; x < Width; x++)
            {
                if (Borders[y, x] == 0)
                {
                    full = false;
                    break;
                }
            }

            if (full)
            {
                lines++;
                for (int row = y; row > 0; row--)
                {
                    for (int col = 0; col < Width; col++)
                    {
                        Borders[row, col] = Borders[row - 1, col];
                    }
                }

                for (int col = 0; col < Width; col++)
                {
                    Borders[0, col] = 0;
                }
                y++;
            }
        }
        return lines;
    }
}