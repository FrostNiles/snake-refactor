using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

//TODO REWRITE INTO CLASSES
// TODO CHANGE GAME redraw
// TODO OVERRIDE BASIC INHERITED METHODS FOR ALL CLASSES

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
            Berry berry = new Berry(gameSettings.screenWidth, gameSettings.screenHeight);

            DateTime time = DateTime.Now;
            DateTime time2 = DateTime.Now;

            // game loop
            while (true)
            {
                Console.Clear();
                // when snake hits the wall
                if (snake.head.XPos == gameSettings.screenWidth - 1 || snake.head.XPos == 0 || snake.head.YPos == gameSettings.screenHeight - 1 || snake.head.YPos == 0)
                {
                    gameSettings.gameover = true;
                }
                gameSettings.DrawGameArea();
                // when snake eats the berry
                if (berry.berryPosition.XPos == snake.head.XPos && berry.berryPosition.YPos == snake.head.YPos)
                {
                    gameSettings.score++;
                    berry.UpdatePosition();
                    
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
                
                snake.Draw();
                berry.Draw();

                time = DateTime.Now;
                // movement
                while (true)
                {
                    time2 = DateTime.Now;
                    if (time2.Subtract(time).TotalMilliseconds > 500) 
                    { 
                        break; 
                    }
                    gameSettings.gameover = snake.Move();
                    //break;
                    // with this break the snake moves only once and the condition for gameover works

                }
                snake.body.Add(new Pixel(snake.head.XPos, snake.head.YPos, ConsoleColor.Red));
                snake.changePosition();
                
                if (snake.body.Count() > gameSettings.score)
                {
                    snake.body.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(gameSettings.screenWidth / 5, gameSettings.screenHeight / 2);
            Console.WriteLine("Game over, Score: " + gameSettings.score);
            Console.SetCursorPosition(gameSettings.screenWidth / 5, gameSettings.screenHeight / 2 + 1);
        }

        static void drawCube()
        {
            Console.Write("■");
        }

        static int getRandomNumber(int min, int max)
        {
            Random randNumber = new Random();
            int randX = randNumber.Next(min, max);
            return randX;
        }

        class Berry
        {
            // TODO do I want to have one berry or multiple berries?
            // TODO up to that I need random position for the berry or every berry
            public Berry(int screenWidth, int screenHeight)
            {
                this.screenWidth = screenWidth;
                this.screenHeight = screenHeight;
                this.berryPosition = new Pixel(getRandomNumber(1, screenWidth), getRandomNumber(1, screenHeight), ConsoleColor.Cyan);
            }

            public Pixel berryPosition { get; set; }
            int screenHeight { get; set; }
            int screenWidth { get; set; }


            public void Draw()
            {
                Console.SetCursorPosition(berryPosition.XPos, berryPosition.YPos);
                Console.ForegroundColor = ConsoleColor.Cyan;
                drawCube();
            }

            public void UpdatePosition()
            {
                berryPosition.XPos = getRandomNumber(1, screenWidth);
                berryPosition.YPos = getRandomNumber(1, screenHeight);
            }

            // TODO EAT BERRY
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
                drawCube();
            }


            public void Eat()
            {
                // TODO
            }

            public bool Move()
            {
                //TODO NOT CHECKING BUTTON PRESSED ANYMORE ...
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo toets = Console.ReadKey(false);
                    if (toets.Key.Equals(ConsoleKey.UpArrow))
                    {
                        Console.WriteLine(toets.Key);
                        if (this.direction != Direction.Down)
                        {
                            this.direction = Direction.Up;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    if (toets.Key.Equals(ConsoleKey.DownArrow))
                    {
                        if (this.direction != Direction.Up)
                        {
                            this.direction = Direction.Down;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                        
                    }
                    if (toets.Key.Equals(ConsoleKey.LeftArrow))
                    {
                        if (this.direction != Direction.Right)
                        {
                            this.direction = Direction.Left;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                        
                    }
                    if (toets.Key.Equals(ConsoleKey.RightArrow))
                    {
                        if (this.direction != Direction.Left)
                        {
                            this.direction = Direction.Right;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
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
                    drawCube();
                    Console.SetCursorPosition(i, this.screenHeight - 1);
                    drawCube();
                }
                for (int i = 0; i < this.screenHeight; i++)
                {
                    Console.SetCursorPosition(0, i);
                    drawCube();
                    Console.SetCursorPosition(this.screenWidth - 1, i);
                    drawCube();
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
}