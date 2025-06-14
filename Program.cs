
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

    enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}
