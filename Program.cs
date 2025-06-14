
namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }

        public static void DrawCube()
        {
            Console.Write("■");
        }
    }

    class Game
    {
        private readonly GameSettings gameSettings;
        private readonly Snake snake;
        private readonly Berry berry;
        private readonly Random rand;
        private DateTime time;
        private DateTime time2;

        public Game()
        {
            gameSettings = new GameSettings(64, 32, 5, false);
            rand = new Random();

            Pixel head = new Pixel(gameSettings.ScreenWidth / 2, gameSettings.ScreenHeight / 2, ConsoleColor.Red);
            snake = new Snake(Direction.Right, head);
            berry = new Berry(gameSettings.ScreenWidth, gameSettings.ScreenHeight);
            berry.Respawn(rand);
        }

        public void Run()
        {
            while (!gameSettings.GameOver)
            {
                Console.Clear();

                if (snake.IsCollidingWithWall(gameSettings.ScreenWidth, gameSettings.ScreenHeight))
                {
                    gameSettings.GameOver = true;
                    break;
                }

                gameSettings.DrawGameArea();

                if (berry.Position.X == snake.Head.X && berry.Position.Y == snake.Head.Y)
                {
                    gameSettings.Score++;
                    berry.Respawn(rand);
                }

                if (snake.IsCollidingWithSelf())
                {
                    gameSettings.GameOver = true;
                    break;
                }

                snake.Draw();
                berry.Draw();

                time = DateTime.Now;

                while (true)
                {
                    time2 = DateTime.Now;
                    if (time2.Subtract(time).TotalMilliseconds > 500)
                        break;

                    snake.HandleInput();
                }

                snake.MoveForward();
                snake.TrimToLength(gameSettings.Score);
            }

            Console.SetCursorPosition(gameSettings.ScreenWidth / 5, gameSettings.ScreenHeight / 2);
            Console.WriteLine("Game over, Score: " + gameSettings.Score);
            Console.SetCursorPosition(gameSettings.ScreenWidth / 5, gameSettings.ScreenHeight / 2 + 1);
        }
    }

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

    class Berry
    {
        private readonly int screenWidth;
        private readonly int screenHeight;

        public Pixel Position { get; private set; }

        public Berry(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.Position = new Pixel(0, 0, ConsoleColor.Cyan);
        }

        public void Draw()
        {
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.ForegroundColor = Position.Color;
            Program.DrawCube();
        }

        public void Respawn(Random rand)
        {
            int x = rand.Next(1, screenWidth - 1);
            int y = rand.Next(1, screenHeight - 1);
            Position = new Pixel(x, y, ConsoleColor.Cyan);
        }
    }

    class GameSettings
    {
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public int Score { get; set; }
        public bool GameOver { get; set; }

        public GameSettings()
        {
            ScreenWidth = 64;
            ScreenHeight = 32;
            Score = 0;
            GameOver = false;
        }

        public GameSettings(int width, int height, int score, bool gameOver)
        {
            ScreenWidth = width;
            ScreenHeight = height;
            Score = score;
            GameOver = gameOver;
        }

        public void DrawGameArea()
        {
            for (int i = 0; i < ScreenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Program.DrawCube();
                Console.SetCursorPosition(i, ScreenHeight - 1);
                Program.DrawCube();
            }

            for (int i = 0; i < ScreenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Program.DrawCube();
                Console.SetCursorPosition(ScreenWidth - 1, i);
                Program.DrawCube();
            }

            Console.ForegroundColor = ConsoleColor.Green;
        }
    }

    class Pixel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleColor Color { get; set; }

        public Pixel(int x, int y, ConsoleColor color)
        {
            X = x;
            Y = y;
            Color = color;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Pixel other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"Pixel({X}, {Y}, Color: {Color})";
        }
    }

    enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}
