namespace Core.Shapes;

public class TShape : Shape
{
    public TShape()
    {
        Blocks = new int[,] { { 1, 1, 0 }, { 0, 1, 1 } };
    }
}