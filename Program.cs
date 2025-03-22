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
            GameSettings gameSettings = new GameSettings(64, 32, 5, false);


            Random randNum = new Random();

            Pixel head = new Pixel(gameSettings.screenWidth / 2, gameSettings.screenHeight / 2, ConsoleColor.Red);
            Snake snake = new Snake(Direction.Right, head);          

            int xPosBerry = randNum.Next(0, gameSettings.screenWidth);
            int YPosBerry = randNum.Next(0, gameSettings.screenHeight);

            DateTime time = DateTime.Now;
            DateTime time2 = DateTime.Now;

            Direction movement = Direction.Right;
            bool buttonPressed = false;

            while (true)
            {
                Console.Clear();
                // when snake hits the wall
                if (snake.head.XPos == gameSettings.screenWidth - 1 || snake.head.XPos == 0 || snake.head.YPos == gameSettings.screenHeight - 1 || snake.head.YPos == 0)
                {
                    gameSettings.gameover = true;
                }
                drawGame(gameSettings.screenWidth, gameSettings.screenHeight);
                
                // when snake eats the berry
                if (xPosBerry == snake.head.XPos && YPosBerry == snake.head.YPos)
                {
                    gameSettings.score++;
                    xPosBerry = randNum.Next(1, gameSettings.screenWidth - 2);
                    YPosBerry = randNum.Next(1, gameSettings.screenHeight - 2);
                }
                // when snake hits itself
                for (int i = 0; i < snake.body.Count(); i++)
                {
                    Console.SetCursorPosition(snake.body[i].XPos, snake.body[i].YPos);
                    drawCube();
                    if (snake.body[i].XPos == snake.head.XPos && snake.body[i].YPos == snake.head.YPos)
                    {
                        gameSettings.gameover = true;
                    }
                }
                if (gameSettings.gameover == true)
                {
                    break;
                }
                // draw snake
                Console.SetCursorPosition(snake.head.XPos, snake.head.YPos);
                Console.ForegroundColor = snake.head.ScreenColor;
                drawCube();
                // draw berry
                Console.SetCursorPosition(xPosBerry, YPosBerry);
                Console.ForegroundColor = ConsoleColor.Cyan;
                drawCube();
                time = DateTime.Now;
                buttonPressed = false;
                // movement
                while (true)
                {
                    time2 = DateTime.Now;
                    if (time2.Subtract(time).TotalMilliseconds > 200) 
                    { 
                        break; 
                    }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo toets = Console.ReadKey(true);
                        //Console.WriteLine(toets.Key.ToString());
                        if (toets.Key.Equals(ConsoleKey.UpArrow) && movement != Direction.Down && buttonPressed == false)
                        {
                            movement = Direction.Up;
                            buttonPressed = true;
                        }
                        if (toets.Key.Equals(ConsoleKey.DownArrow) && movement != Direction.Up && buttonPressed == false)
                        {
                            movement = Direction.Down;
                            buttonPressed = true;
                        }
                        if (toets.Key.Equals(ConsoleKey.LeftArrow) && movement != Direction.Right && buttonPressed == false)
                        {
                            movement = Direction.Left;
                            buttonPressed = true;
                        }
                        if (toets.Key.Equals(ConsoleKey.RightArrow) && movement != Direction.Left && buttonPressed == false)
                        {
                            movement = Direction.Right;
                            buttonPressed = true;
                        }
                    }
                }
                snake.body.Add(new Pixel(snake.head.XPos, snake.head.YPos, ConsoleColor.Red));
                snake.changePosition(movement);
                
                if (snake.body.Count() > gameSettings.score)
                {
                    snake.body.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(gameSettings.screenWidth / 5, gameSettings.screenHeight / 2);
            Console.WriteLine("Game over, Score: " + gameSettings.score);
            Console.SetCursorPosition(gameSettings.screenWidth / 5, gameSettings.screenHeight / 2 + 1);
        }

        static void drawGame(int screenWidth, int screenHeight)
        {
            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                drawCube();
                Console.SetCursorPosition(i, screenHeight - 1);
                drawCube();
            }
            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                drawCube();
                Console.SetCursorPosition(screenWidth - 1, i);
                drawCube();
            }
            Console.ForegroundColor = ConsoleColor.Green;
        }

        static void drawCube()
        {
            Console.Write("■");
        }

        class Berry
        {
            // TODO do I want to have one berry or multiple berries?
            // TODO up to that I need random position for the berry or every berry

            
//            Random randNumber = new Random();
  //          var randX = randNumber.Next(1, 63); // TODO rewrite this from numbers to gameSettings
    //        var randY = randNumber.Next(1, 31);
      //      Pixel berryPosition = new Pixel(randX, randY, ConsoleColor.Cyan);
            // TODO UPDATE POSITION
            // TODO DRAW BERRY
            // TODO EAT BERRY
        }


        // TODO OVERRIDE BASIC INHERITED METHODS FOR ALL CLASSES
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
                // TODO
            }


            public void Eat()
            {
                // TODO
            }

            public void changePosition(Direction direction)
            {
                switch (direction)
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
}