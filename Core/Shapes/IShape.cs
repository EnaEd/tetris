namespace Core.Shapes;

public class IShape : Shape
{
    public IShape()
    {
        Blocks = new int[,] { { 1, 1, 1, 1 } };
    }
}