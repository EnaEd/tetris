namespace Core.Shapes;

public class LShape : Shape
{
    public LShape()
    {
        Blocks = new int[,] { { 0, 0, 1 }, { 1, 1, 1 } };
    }
}