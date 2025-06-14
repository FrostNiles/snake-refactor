
namespace Snake
{
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
            Console.ForegroundColor = ConsoleColor.Green;

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
        }
        public override string ToString()
        {
            return $"Screen: {ScreenWidth}x{ScreenHeight}, Score: {Score}, Game Over: {GameOver}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ScreenWidth, ScreenHeight, Score, GameOver);
        }

        public override bool Equals(object obj)
        {
            if (obj is GameSettings other)
            {
                return ScreenWidth == other.ScreenWidth &&
                       ScreenHeight == other.ScreenHeight &&
                       Score == other.Score &&
                       GameOver == other.GameOver;
            }
            return false;
    }
}
