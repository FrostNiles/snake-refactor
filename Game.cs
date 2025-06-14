using System.Diagnostics;

namespace Snake
{
    class Game
    {
        private readonly GameSettings gameSettings;
        private readonly Snake snake;
        private readonly Berry berry;
        private readonly Random rand;
        private readonly Stopwatch stopwatch;
        private const int UpdateIntervalMs = 200;

        public Game()
        {
            gameSettings = new GameSettings(64, 32, 5, false);
            rand = new Random();
            stopwatch = new Stopwatch();

            Pixel head = new Pixel(gameSettings.ScreenWidth / 2, gameSettings.ScreenHeight / 2, ConsoleColor.Red);
            snake = new Snake(Direction.Right, head);
            berry = new Berry(gameSettings.ScreenWidth, gameSettings.ScreenHeight);
            berry.Respawn(rand);
            gameSettings.DrawGameArea();
        }

        public void Run()
        {
            stopwatch.Start();

            while (!gameSettings.GameOver)
            {
                if (stopwatch.ElapsedMilliseconds < UpdateIntervalMs)
                {
                    Thread.Sleep(1);
                    snake.HandleInput();
                    continue;
                }

                UpdateGame();
                stopwatch.Restart();
            }

            EndGame();
        }

        private void UpdateGame()
        {
            if (snake.IsCollidingWithWall(gameSettings.ScreenWidth, gameSettings.ScreenHeight))
            {
                gameSettings.GameOver = true;
                return;
            }

            if (berry.Position.X == snake.Head.X && berry.Position.Y == snake.Head.Y)
            {
                gameSettings.Score++;
                berry.Respawn(rand);
            }

            if (snake.IsCollidingWithSelf())
            {
                gameSettings.GameOver = true;
                return;
            }

            snake.EraseTail();
            snake.MoveForward();
            snake.TrimToLength(gameSettings.Score);

            snake.Draw();
            berry.Draw();
        }

        private void EndGame()
        {
            Console.SetCursorPosition(gameSettings.ScreenWidth / 5, gameSettings.ScreenHeight / 2);
            Console.WriteLine("Game over, Score: " + gameSettings.Score);
            Console.SetCursorPosition(gameSettings.ScreenWidth / 5, gameSettings.ScreenHeight / 2 + 1);
        }
        public override string ToString()
        {
            return $"Game Settings: {gameSettings}, Snake: {snake}, Berry: {berry}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(gameSettings, snake, berry, rand, stopwatch);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Game other)
            {
                return gameSettings.Equals(other.gameSettings) &&
                       snake.Equals(other.snake) &&
                       berry.Equals(other.berry) &&
                       rand.Equals(other.rand) &&
                       stopwatch.ElapsedMilliseconds == other.stopwatch.ElapsedMilliseconds;
            }
            return false;
        }
    }

}
