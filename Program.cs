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

                CheckWallCollision();
                gameSettings.DrawGameArea();
                CheckBerryEaten();
                DrawSnakeBodyAndCheckSelfCollision();

                if (gameSettings.gameover)
                    break;

                snake.Draw();
                berry.Draw();

                time = DateTime.Now;

                while (true)
                {
                    time2 = DateTime.Now;
                    if (time2.Subtract(time).TotalMilliseconds > 500) break;

                    gameSettings.gameover = snake.Move();
                }

                snake.body.Add(new Pixel(snake.head.XPos, snake.head.YPos, ConsoleColor.Red));
                snake.changePosition();

                if (snake.body.Count > gameSettings.score)
                {
                    snake.body.RemoveAt(0);
                }
            }

            EndGame();
        }

        private void CheckWallCollision()
        {
            if (snake.head.XPos == gameSettings.screenWidth - 1 ||
                snake.head.XPos == 0 ||
                snake.head.YPos == gameSettings.screenHeight - 1 ||
                snake.head.YPos == 0)
            {
                gameSettings.gameover = true;
            }
        }

        private void CheckBerryEaten()
        {
            if (berry.berryPosition.XPos == snake.head.XPos &&
                berry.berryPosition.YPos == snake.head.YPos)
            {
                gameSettings.score++;
                berry.UpdatePosition();
            }
        }

        private void DrawSnakeBodyAndCheckSelfCollision()
        {
            foreach (var segment in snake.body)
            {
                Console.SetCursorPosition(segment.XPos, segment.YPos);
                Program.DrawCube();

                if (segment.XPos == snake.head.XPos && segment.YPos == snake.head.YPos)
                {
                    gameSettings.gameover = true;
                }
            }
        }

        private void EndGame()
        {
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
        public Snake(Direction direction, Pixel head)
        {
            this.direction = direction;
            this.head = head;
            this.body = new List<Pixel>();
        }

        public Direction direction { get; set; }
        public Pixel head { get; set; }
        public List<Pixel> body { get; set; }

        public void Draw()
        {
            Console.SetCursorPosition(head.XPos, head.YPos);
            Console.ForegroundColor = head.ScreenColor;
            Program.DrawCube();
        }

        public bool Move()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(false);
                if (key.Key == ConsoleKey.UpArrow && this.direction != Direction.Down)
                {
                    this.direction = Direction.Up;
                }
                else if (key.Key == ConsoleKey.DownArrow && this.direction != Direction.Up)
                {
                    this.direction = Direction.Down;
                }
                else if (key.Key == ConsoleKey.LeftArrow && this.direction != Direction.Right)
                {
                    this.direction = Direction.Left;
                }
                else if (key.Key == ConsoleKey.RightArrow && this.direction != Direction.Left)
                {
                    this.direction = Direction.Right;
                }
                return false;
            }
            return false;
        }

        public void changePosition()
        {
            switch (this.direction)
            {
                case Direction.Up:
                    head.YPos--;
                    break;
                case Direction.Down:
                    head.YPos++;
                    break;
                case Direction.Left:
                    head.XPos--;
                    break;
                case Direction.Right:
                    head.XPos++;
                    break;
            }
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
