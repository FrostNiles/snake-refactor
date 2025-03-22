using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 32;
            Console.WindowWidth = 64;

            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;
            Random randNum = new Random();

            int score = 5;
            bool gameover = false;

            Pixel head = new Pixel(screenWidth/2, screenHeight/2, ConsoleColor.Red);

            List<int> xPosBody = new List<int>();
            List<int> YPosBody = new List<int>();

            int xPosBerry = randNum.Next(0, screenWidth);
            int YPosBerry = randNum.Next(0, screenHeight);

            DateTime time = DateTime.Now;
            DateTime time2 = DateTime.Now;

            string movement = "RIGHT";
            string buttonPressed = "no";

            while (true)
            {
                Console.Clear();
                if (head.XPos == screenWidth - 1 || head.XPos == 0 || head.YPos == screenHeight - 1 || head.YPos == 0)
                {
                    gameover = true;
                }
                for (int i = 0; i < screenWidth; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write("■");
                }
                for (int i = 0; i < screenWidth; i++)
                {
                    Console.SetCursorPosition(i, screenHeight - 1);
                    Console.Write("■");
                }
                for (int i = 0; i < screenHeight; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("■");
                }
                for (int i = 0; i < screenHeight; i++)
                {
                    Console.SetCursorPosition(screenWidth - 1, i);
                    Console.Write("■");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                if (xPosBerry == head.XPos && YPosBerry == head.YPos)
                {
                    score++;
                    xPosBerry = randNum.Next(1, screenWidth - 2);
                    YPosBerry = randNum.Next(1, screenHeight - 2);
                }
                for (int i = 0; i < xPosBody.Count(); i++)
                {
                    Console.SetCursorPosition(xPosBody[i], YPosBody[i]);
                    Console.Write("■");
                    if (xPosBody[i] == head.XPos && YPosBody[i] == head.YPos)
                    {
                        gameover = true;
                    }
                }
                if (gameover)
                {
                    break;
                }
                Console.SetCursorPosition(head.XPos, head.YPos);
                Console.ForegroundColor = head.ScreenColor;
                Console.Write("■");
                Console.SetCursorPosition(xPosBerry, YPosBerry);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("■");
                time = DateTime.Now;
                buttonPressed = "no";
                while (true)
                {
                    time2 = DateTime.Now;
                    if (time2.Subtract(time).TotalMilliseconds > 500) { break; }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo toets = Console.ReadKey(true);
                        //Console.WriteLine(toets.Key.ToString());
                        if (toets.Key.Equals(ConsoleKey.UpArrow) && movement != "DOWN" && buttonPressed == "no")
                        {
                            movement = "UP";
                            buttonPressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.DownArrow) && movement != "UP" && buttonPressed == "no")
                        {
                            movement = "DOWN";
                            buttonPressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.LeftArrow) && movement != "RIGHT" && buttonPressed == "no")
                        {
                            movement = "LEFT";
                            buttonPressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.RightArrow) && movement != "LEFT" && buttonPressed == "no")
                        {
                            movement = "RIGHT";
                            buttonPressed = "yes";
                        }
                    }
                }
                xPosBody.Add(head.XPos);
                YPosBody.Add(head.YPos);
                switch (movement)
                {
                    case "UP":
                        head.YPos--;
                        break;
                    case "DOWN":
                        head.YPos++;
                        break;
                    case "LEFT":
                        head.XPos--;
                        break;
                    case "RIGHT":
                        head.XPos++;
                        break;
                }
                if (xPosBody.Count() > score)
                {
                    xPosBody.RemoveAt(0);
                    YPosBody.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
        }

        class Pixel
        {
            public Pixel(int xPos, int yPos, ConsoleColor color)
            {
                XPos = xPos;
                YPos = yPos;
                ScreenColor = color;
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
}