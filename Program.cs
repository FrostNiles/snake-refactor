﻿using System;
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


            Pixel head = new Pixel(gameSettings.screenWidth/2, gameSettings.screenHeight/2, ConsoleColor.Red);

            List<int> xPosBody = new List<int>();
            List<int> YPosBody = new List<int>();

            int xPosBerry = randNum.Next(0, gameSettings.screenWidth);
            int YPosBerry = randNum.Next(0, gameSettings.screenHeight);

            DateTime time = DateTime.Now;
            DateTime time2 = DateTime.Now;

            Direction movement = Direction.Right;
            bool buttonPressed = false;

            while (true)
            {
                Console.Clear();
                if (head.XPos == gameSettings.screenWidth - 1 || head.XPos == 0 || head.YPos == gameSettings.screenHeight - 1 || head.YPos == 0)
                {
                    gameSettings.gameover = true;
                }
                for (int i = 0; i < gameSettings.screenWidth; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    drawCube();
                }
                for (int i = 0; i < gameSettings.screenWidth; i++)
                {
                    Console.SetCursorPosition(i, gameSettings.screenHeight - 1);
                    drawCube();
                }
                for (int i = 0; i < gameSettings.screenHeight; i++)
                {
                    Console.SetCursorPosition(0, i);
                    drawCube();
                }
                for (int i = 0; i < gameSettings.screenHeight; i++)
                {
                    Console.SetCursorPosition(gameSettings.screenWidth - 1, i);
                    drawCube();
                }
                Console.ForegroundColor = ConsoleColor.Green;
                if (xPosBerry == head.XPos && YPosBerry == head.YPos)
                {
                    gameSettings.score++;
                    xPosBerry = randNum.Next(1, gameSettings.screenWidth - 2);
                    YPosBerry = randNum.Next(1, gameSettings.screenHeight - 2);
                }
                for (int i = 0; i < xPosBody.Count(); i++)
                {
                    Console.SetCursorPosition(xPosBody[i], YPosBody[i]);
                    drawCube();
                    if (xPosBody[i] == head.XPos && YPosBody[i] == head.YPos)
                    {
                        gameSettings.gameover = true;
                    }
                }
                if (gameSettings.gameover == true)
                {
                    break;
                }
                Console.SetCursorPosition(head.XPos, head.YPos);
                Console.ForegroundColor = head.ScreenColor;
                drawCube();
                Console.SetCursorPosition(xPosBerry, YPosBerry);
                Console.ForegroundColor = ConsoleColor.Cyan;
                drawCube();
                time = DateTime.Now;
                buttonPressed = false;
                while (true)
                {
                    time2 = DateTime.Now;
                    if (time2.Subtract(time).TotalMilliseconds > 500) 
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
                xPosBody.Add(head.XPos);
                YPosBody.Add(head.YPos);
                switch (movement)
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
                if (xPosBody.Count() > gameSettings.score)
                {
                    xPosBody.RemoveAt(0);
                    YPosBody.RemoveAt(0);
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

        class Berry
        {
            // TODO do I want to have one berry or multiple berries?
            // TODO up to that I need random position for the berry or every berry
            public Berry(int xPos, int yPos, ConsoleColor color)
            {
                this.XPos = xPos;
                this.YPos = yPos;
                this.ScreenColor = color;
            }

            public Pixel berryPosition { get; set; }
            
            Random randNumber = new Random();
            var randX = randNumber.Next(1, 63); // TODO rewrite this from numbers to gameSettings
            var randY = randNumber.Next(1, 31);
            Pixel berryPosition = new Pixel(randX, randY, ConsoleColor.Cyan);
            // TODO UPDATE POSITION
            // TODO DRAW BERRY
            // TODO EAT BERRY
        }

        class Snake
        {
            public Snake(Direction direction)
            {
                this.direction = direction;
            }

            public Direction direction { get; set; }

            Pixel head = new Pixel(16, 10, ConsoleColor.Red); // TODO random position - in the middle of the screen
            
            List<Pixel> body = new List<Pixel>();
            

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