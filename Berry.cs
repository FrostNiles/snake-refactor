
namespace Snake
{
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

        public override string ToString()
        {
            return $"Berry Position: ({Position.X}, {Position.Y})";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position.X, Position.Y, Position.Color);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Berry other)
            {
                return Position.Equals(other.Position);
            }
            return false;
        }
    }
}
