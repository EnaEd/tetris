using System.Diagnostics;
using System.Text;
using Ardalis.GuardClauses;
using Core.Board;
using Core.Shapes;

Console.CursorVisible = false;
Console.Title = "Console Tetris C#";

int fallDelay = 500; 
var board = new Board();
board.SpawnShape();
Guard.Against.Null(board.CurrentShape);

Stopwatch timer = Stopwatch.StartNew();

while (true)
{
    if (Console.KeyAvailable)
    {
        ConsoleKey key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.Escape)
        {
            break;
        }

        if (key == ConsoleKey.LeftArrow && !board.CheckCollision(board.CurrentShape.X - 1, board.CurrentShape.Y))
        {
            board.CurrentShape.MoveLeft();
        }

        if (key == ConsoleKey.RightArrow && !board.CheckCollision(board.CurrentShape.X + 1, board.CurrentShape.Y))
        {
            board.CurrentShape.MoveRight();
        }

        if (key == ConsoleKey.DownArrow && !board.CheckCollision( board.CurrentShape.X, board.CurrentShape.Y + 1))
        {
            board.CurrentShape.MoveDown();
        }
        
        if (key == ConsoleKey.UpArrow)
        {
            board.RotateShape();
        }
        
        if (key == ConsoleKey.Spacebar)
        {
            while (!board.CheckCollision(board.CurrentShape.X, board.CurrentShape.Y + 1))
            {
                board.CurrentShape.MoveDown();
            }
            timer.Restart();
            if (!board.LockAndSpawn())
            {
                break;
            }
        }
    }

    if (timer.ElapsedMilliseconds >= fallDelay)
    {
        if (!board.CheckCollision(board.CurrentShape.X, board.CurrentShape.Y + 1))
        {
            board.CurrentShape.MoveDown();
        }
        else
        {
            if (!board.LockAndSpawn())
            {
                break; 
            }
            
            fallDelay = Math.Max(100, 500 - (board.TotalScore / 500) * 40);
        }

        timer.Restart();
    }

    Draw();
    Thread.Sleep(20);
}

Console.SetBufferSize(Math.Max(Console.BufferWidth, Board.Width * 2 + 10), Math.Max(Console.BufferHeight, Board.Height + 5));
Console.SetCursorPosition(0, Board.Height + 2);
Console.WriteLine($"ИГРА ОКОНЧЕНА! Итоговый счёт: {board.TotalScore}");
Console.CursorVisible = true;
return;

void Draw()
{
    Guard.Against.Null(board.CurrentShape, nameof(Shape));

    Console.SetCursorPosition(0, 0);
    var sb = new StringBuilder();

    int rows = board.CurrentShape.Blocks.GetLength(0);
    int cols = board.CurrentShape.Blocks.GetLength(1);

    for (int y = 0; y < Board.Height; y++)
    {
        sb.Append("<!");
        for (int x = 0; x < Board.Width; x++)
        {
            bool isPieceCell = false;

            if (y >= board.CurrentShape.Y &&
                y < board.CurrentShape.Y + rows && x >= board.CurrentShape.X &&
                x < board.CurrentShape.X + cols)
            {
                if (board.CurrentShape.Blocks[y - board.CurrentShape.Y, x - board.CurrentShape.X] == 1)
                {
                    isPieceCell = true;
                }
            }

            var pic = isPieceCell || board.Borders[y, x] == 1 ? "[]" : " .";
            sb.Append(pic);
        }

        sb.Append("!>");

        if (y == 2) sb.Append($"   Счёт: {board.TotalScore}");
        if (y == 5) sb.Append("   Управление:");
        if (y == 6) sb.Append("   <- -> : Движение");
        if (y == 7) sb.Append("   Вверх : Поворот");
        if (y == 8) sb.Append("   Вниз  : Ускорить");
        if (y == 9) sb.Append("   Пробел: Сбросить");
        if (y == 10) sb.Append("   Esc   : Выход");

        sb.AppendLine();
    }

    sb.AppendLine("<!" + new string('=', Board.Width * 2) + "!>");
    Console.Write(sb.ToString());
}