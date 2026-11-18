namespace Core.Shapes;

public class ZShape : Shape
{
    public ZShape()
    {
        Blocks = new int[,] { { 1, 1, 0 }, { 0, 1, 1 } };
    }
}