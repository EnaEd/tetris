namespace Core.Shapes;

public class JShape : Shape
{
    public JShape()
    {
        Blocks = new[,] { { 1, 0, 0 }, { 1, 1, 1 } };
    }
}