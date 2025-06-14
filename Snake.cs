
namespace Snake
{
    class Snake
    {
        public Direction CurrentDirection { get; private set; }
        public Pixel Head { get; private set; }
        private readonly List<Pixel> body;

        public IReadOnlyList<Pixel> Body => body;

        public Snake(Direction direction, Pixel head)
        {
            CurrentDirection = direction;
            Head = head;
            body = new List<Pixel>();
        }

        public void Draw()
        {
            DrawHead();
            DrawBody();
        }

        private void DrawHead()
        {
            Console.SetCursorPosition(Head.X, Head.Y);
            Console.ForegroundColor = Head.Color;
            Program.DrawCube();
        }

        private void DrawBody()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var segment in body)
            {
                Console.SetCursorPosition(segment.X, segment.Y);
                Program.DrawCube();
            }
        }

        public void EraseTail()
        {
            if (body.Count == 0)
            {
                return;
            }

            var tail = body[0];
            Console.SetCursorPosition(tail.X, tail.Y);
            Console.Write(" ");
        }

        public void HandleInput()
        {
            if (!Console.KeyAvailable) return;

            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow when CurrentDirection != Direction.Down:
                    CurrentDirection = Direction.Up;
                    break;
                case ConsoleKey.DownArrow when CurrentDirection != Direction.Up:
                    CurrentDirection = Direction.Down;
                    break;
                case ConsoleKey.LeftArrow when CurrentDirection != Direction.Right:
                    CurrentDirection = Direction.Left;
                    break;
                case ConsoleKey.RightArrow when CurrentDirection != Direction.Left:
                    CurrentDirection = Direction.Right;
                    break;
            }
        }

        public void MoveForward()
        {
            body.Add(new Pixel(Head.X, Head.Y, Head.Color));

            Head = CurrentDirection switch
            {
                Direction.Up => new Pixel(Head.X, Head.Y - 1, ConsoleColor.Red),
                Direction.Down => new Pixel(Head.X, Head.Y + 1, ConsoleColor.Red),
                Direction.Left => new Pixel(Head.X - 1, Head.Y, ConsoleColor.Red),
                Direction.Right => new Pixel(Head.X + 1, Head.Y, ConsoleColor.Red),
                _ => Head
            };
        }

        public void TrimToLength(int length)
        {
            while (body.Count > length)
            {
                body.RemoveAt(0);
            }
        }

        public bool IsCollidingWithSelf()
        {
            return body.Any(segment => segment.X == Head.X && segment.Y == Head.Y);
        }

        public bool IsCollidingWithWall(int width, int height)
        {
            return Head.X <= 0 || Head.X >= width - 1 ||
                   Head.Y <= 0 || Head.Y >= height - 1;
        }
    }
}
