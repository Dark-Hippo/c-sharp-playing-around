﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace GameEngine
{
    public class Animate
    {
        private const int WIDTH = 40;
        private const int HEIGHT = 40;

        private const char PLAYER_CHARACTER = 'A';
        private const char OBSTACLE_CHARACTER = 'O';
        private const char EXPLOSION_CHARACTER = 'x';

        private const int ARENA_WIDTH = 10;
        private const int ARENA_HEIGHT = 14;
        private const char ARENA_WALL_CHARACTER = '|';
        private const char ARENA_ENDWALL_CHARACTER = '-';
        private const char ARENA_FLOOR_CHARACTER = ' ';

        private const int FRAMES_PER_SECOND = 10;
        private const int SCORE_Y_OFFSET = 2;
        private const int LOG_Y_OFFSET = 4;
        private const int LEVEL_THRESHOLD = 200;
        private const int INITIAL_LEVEL = 1;
        
        private enum GameState
        {
            Won,
            Lost,
            Running
        }

        private GameState gameState;
        private Player player;
        private Environment arena;
        private List<GameObject> obstacles;

        private int framesPerSecond, sleepTime;
        private int level;
        private Int64 score;
        private bool flip;
        private bool randomObstacleDistribution;

        public Animate()
        {
            setup();
            reset();
        }

        public void Go()
        {
            arena.DrawArena();
            startDraw();
            startKeyboardInput();
        }

        private void setup()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(WIDTH, HEIGHT);
            Console.SetBufferSize(WIDTH, HEIGHT);

            arena = new Environment(ARENA_WIDTH, ARENA_HEIGHT);
            arena.SideWall = ARENA_WALL_CHARACTER;
            arena.EndWall = ARENA_ENDWALL_CHARACTER;
            arena.Floor = ARENA_FLOOR_CHARACTER;

            
            player = new Player((arena.Width / 2), arena.Height);
            player.Character = PLAYER_CHARACTER;

            framesPerSecond = FRAMES_PER_SECOND;
            sleepTime = 1000 / framesPerSecond;

            flip = false;
        }

        private void reset()
        {
            player.State = GameObject.ObstacleState.Active;
            gameState = GameState.Running;
            randomObstacleDistribution = new Random().NextDouble() > 0.5;

            player.ResetPosition();

            score = 0;
            clearScore();
            clearLog();

            level = INITIAL_LEVEL;
            obstacles = new List<GameObject>();
            removeDeactiveAndGenerateNewObstacles();
        }

        private void startDraw()
        {
            Thread t = new Thread(draw);
            t.Start();
        }

        private void startKeyboardInput()
        {
            ConsoleKeyInfo consoleKeyInfo = new ConsoleKeyInfo();

            while (consoleKeyInfo.Key != ConsoleKey.Q)
            {
                if(Console.KeyAvailable)
                {
                    consoleKeyInfo = Console.ReadKey(true);
                }

                if(consoleKeyInfo.Key == ConsoleKey.A)
                {
                    moveLeft();
                } else if(consoleKeyInfo.Key == ConsoleKey.D)
                {
                    moveRight();
                } else if (consoleKeyInfo.Key == ConsoleKey.W)
                {
                    moveUp();
                } else if (consoleKeyInfo.Key == ConsoleKey.S)
                {
                    moveDown();
                } else if (consoleKeyInfo.Key == ConsoleKey.R && gameState != GameState.Running)
                {
                    reset();
                    Go();
                }
                consoleKeyInfo = default(ConsoleKeyInfo);
            }
        }
        
        /// <summary>
        /// Main game loop
        /// </summary>
        private void draw()
        {
            while (gameState == GameState.Running)
            {
                update();
                render();
                Thread.Sleep(sleepTime);
            }
            drawLog("Game over. Press 'r' to restart.");
        }

        /// <summary>
        /// Calls functions to draw stuff to the screen
        /// </summary>
        private void render()
        {
            arena.DrawArena();
            drawObstacles();
            drawPlayer();
            drawScore();
        }

        /// <summary>
        /// Calls functions to update the position / state of stuff
        /// </summary>
        private void update()
        {
            if (gameState == GameState.Running)
            {
                flip = !flip;
                moveObstacles();
                checkObstaclesPosition();
                removeDeactiveAndGenerateNewObstacles();
                incrementScore();
                if (score > (level * LEVEL_THRESHOLD))
                {
                    increaseLevel();
                }
                checkCollisions();
            }
        }

        private void drawScore()
        {
            Console.SetCursorPosition(0, arena.Height + SCORE_Y_OFFSET);
            Console.Write("Score: " + score);
        }

        private void drawLog(string message)
        {
            Console.SetCursorPosition(0, arena.Height + LOG_Y_OFFSET);
            Console.Write(message);
        }

        private void drawObstacles()
        {
            foreach (var obstacle in obstacles)
            {
                if (obstacle.State == Obstacle.ObstacleState.Active)
                {
                    Console.SetCursorPosition(obstacle.X, obstacle.Y);
                    Console.Write(obstacle.Character);
                }
            }
        }

        private void drawPlayer()
        {
            Console.SetCursorPosition(player.X, player.Y);
            if (player.State == GameObject.ObstacleState.Active)
                Console.Write(player.Character);
            else
                drawExplosion();
        }

        private void drawExplosion()
        {
            for (int x = player.X - 1; x <= player.X + 1; x++)
            {
                for (int y = player.Y - 1; y <= player.Y + 1; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(EXPLOSION_CHARACTER);
                }
            }
            
        }

        private void clearScore()
        {
            Console.SetCursorPosition(0, arena.Height + SCORE_Y_OFFSET);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        private void clearLog()
        {
            Console.SetCursorPosition(0, arena.Height + LOG_Y_OFFSET);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        private void incrementScore()
        {
            score += (ARENA_HEIGHT - player.Y);
        }

        private void increaseLevel()
        {
            level++;
        }

        private void removeDeactiveAndGenerateNewObstacles()
        {
            if (!randomObstacleDistribution)
            {
                if(obstacles.Count(o => o.State == Obstacle.ObstacleState.Deactive) > 0 || obstacles.Count == 0)
                {
                    obstacles.Clear();
                    generateObstacles(level);
                }
            } else
            {
                obstacles.RemoveAll(o => o.State == Obstacle.ObstacleState.Deactive);

                // always leave one space to move through
                var requiredObstacleCount = Math.Min(level - obstacles.Count, arena.Width - 1);

                generateObstacles(requiredObstacleCount);
            }
        }

        private void generateObstacles(int requiredObstacleCount)
        {
            for (int i = 0; i < requiredObstacleCount; i++)
            {
                var obstacle = generateObstacle();
                while (obstacles.Count(o => o.X == obstacle.X) != 0)
                {
                    obstacle = generateObstacle();
                }
                obstacles.Add(obstacle);
            }
        }

        private Obstacle generateObstacle()
        {
            var startPosition = new Random().Next(1, ARENA_WIDTH - 1);
            var obstacle = new Obstacle(startPosition, 1);
            obstacle.State = Obstacle.ObstacleState.Active;
            obstacle.Character = OBSTACLE_CHARACTER;

            return obstacle;
        }

        private void checkCollisions()
        {
            foreach (var obstacle in obstacles)
            {
                if (obstacle.X == player.X && obstacle.Y == player.Y)
                {
                    obstacle.State = Obstacle.ObstacleState.Deactive;
                    gameState = GameState.Lost;
                    player.State = GameObject.ObstacleState.Deactive;
                }
            }
        }

        private void checkObstaclesPosition()
        {
            foreach (var obstacle in obstacles)
            {
                if (obstacle.Y >= arena.Height)
                {
                    obstacle.State = Obstacle.ObstacleState.Deactive;
                }
            }
        }

        private void moveObstacles()
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.Update();
            }
        }

        #region movement functions
        private void setCursor(int x, int y)
        {
            player.X = x;
            player.Y = y;
        }

        private void moveLeft()
        {
            if (player.X > 1)
            {
                player.X--;
            }
        }

        private void moveRight()
        {
            if (player.X < arena.Width)
            {
                player.X++;
            }
        }

        private void moveUp()
        {
            if (player.Y > 1)
            {
                player.Y--;
            }
        }

        private void moveDown()
        {
            if (player.Y < arena.Height)
            {
                player.Y++;
            }
        }
        #endregion
    }
}
