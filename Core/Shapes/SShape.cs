namespace Core.Shapes;

public class SShape : Shape
{
    public SShape()
    {
        Blocks = new int[,] { { 0, 1, 1 }, { 1, 1, 0 } };
    }
}