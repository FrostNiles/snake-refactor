using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

        public static int GetRandomNumber(int min, int max)
        {
            Random randNumber = new Random();
            return randNumber.Next(min, max);
        }
    }

    class Game
    {
        private readonly GameSettings gameSettings;
        private readonly Snake snake;
        private readonly Berry berry;
        private DateTime time;
        private DateTime time2;

        public Game()
        {
            gameSettings = new GameSettings(64, 32, 5, false);
            Pixel head = new Pixel(gameSettings.screenWidth / 2, gameSettings.screenHeight / 2, ConsoleColor.Red);
            snake = new Snake(Direction.Right, head);
            berry = new Berry(gameSettings.screenWidth, gameSettings.screenHeight);
            time = DateTime.Now;
            time2 = DateTime.Now;
        }

        public void Run()
        {
            while (!gameSettings.gameover)
            {
                Console.Clear();

                if (snake.IsCollidingWithWall(gameSettings.screenWidth, gameSettings.screenHeight))
                {
                    gameSettings.gameover = true;
                    break;
                }

                gameSettings.DrawGameArea();

                if (berry.berryPosition.XPos == snake.Head.XPos &&
                    berry.berryPosition.YPos == snake.Head.YPos)
                {
                    gameSettings.score++;
                    berry.UpdatePosition();
                }

                if (snake.IsCollidingWithSelf())
                {
                    gameSettings.gameover = true;
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
                snake.TrimToLength(gameSettings.score);
            }

            Console.SetCursorPosition(gameSettings.screenWidth / 5, gameSettings.screenHeight / 2);
            Console.WriteLine("Game over, Score: " + gameSettings.score);
            Console.SetCursorPosition(gameSettings.screenWidth / 5, gameSettings.screenHeight / 2 + 1);
        }
    }

    class Berry
    {
        public Berry(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.berryPosition = new Pixel(Program.GetRandomNumber(1, screenWidth), Program.GetRandomNumber(1, screenHeight), ConsoleColor.Cyan);
        }

        public Pixel berryPosition { get; set; }
        int screenHeight { get; set; }
        int screenWidth { get; set; }

        public void Draw()
        {
            Console.SetCursorPosition(berryPosition.XPos, berryPosition.YPos);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Program.DrawCube();
        }

        public void UpdatePosition()
        {
            berryPosition.XPos = Program.GetRandomNumber(1, screenWidth);
            berryPosition.YPos = Program.GetRandomNumber(1, screenHeight);
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
            Console.SetCursorPosition(Head.XPos, Head.YPos);
            Console.ForegroundColor = Head.ScreenColor;
            Program.DrawCube();
        }

        private void DrawBody()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var segment in body)
            {
                Console.SetCursorPosition(segment.XPos, segment.YPos);
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
            body.Add(new Pixel(Head.XPos, Head.YPos, Head.ScreenColor));

            Head = CurrentDirection switch
            {
                Direction.Up => new Pixel(Head.XPos, Head.YPos - 1, ConsoleColor.Red),
                Direction.Down => new Pixel(Head.XPos, Head.YPos + 1, ConsoleColor.Red),
                Direction.Left => new Pixel(Head.XPos - 1, Head.YPos, ConsoleColor.Red),
                Direction.Right => new Pixel(Head.XPos + 1, Head.YPos, ConsoleColor.Red),
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
            return body.Any(segment => segment.XPos == Head.XPos && segment.YPos == Head.YPos);
        }

        public bool IsCollidingWithWall(int width, int height)
        {
            return Head.XPos <= 0 || Head.XPos >= width - 1 ||
                   Head.YPos <= 0 || Head.YPos >= height - 1;
        }
    }

    class GameSettings
    {
        public GameSettings()
        {
            this.screenWidth = 64;
            this.screenHeight = 32;
            this.score = 0;
            this.gameover = false;
        }

        public GameSettings(int screenWidth, int screenHeight, int score, bool gameover)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.score = score;
            this.gameover = gameover;
        }

        public int screenWidth { get; set; }
        public int screenHeight { get; set; }
        public int score { get; set; }
        public bool gameover { get; set; }

        public void DrawGameArea()
        {
            for (int i = 0; i < this.screenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Program.DrawCube();
                Console.SetCursorPosition(i, this.screenHeight - 1);
                Program.DrawCube();
            }

            for (int i = 0; i < this.screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Program.DrawCube();
                Console.SetCursorPosition(this.screenWidth - 1, i);
                Program.DrawCube();
            }

            Console.ForegroundColor = ConsoleColor.Green;
        }
    }

    class Pixel
    {
        public Pixel(int xPos, int yPos, ConsoleColor color)
        {
            this.XPos = xPos;
            this.YPos = yPos;
            this.ScreenColor = color;
        }

        public int XPos { get; set; }
        public int YPos { get; set; }
        public ConsoleColor ScreenColor { get; set; }
    }

    enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}
