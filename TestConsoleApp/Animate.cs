using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace GameEngine
{
    public class Animate
    {
        private const int WIDTH = 40;
        private const int HEIGHT = 40;

        private const int ARENA_WIDTH = 10;
        private const int ARENA_HEIGHT = 14;

        private const int SCORE_Y_OFFSET = 2;
        private const int LOG_Y_OFFSET = 4;
        private const int LEVEL_THRESHOLD = 200;
        private const int INITIAL_LEVEL = 1;

        private Engine engine;
        private Player player;
        private Environment environment;
        private ScoreBoard scoreBoard;
        private MessageBoard messageBoard;
        private List<Obstacle> obstacles;

        private int sleepTime;
        private int level;
        private bool flip;
        private bool randomObstacleDistribution;

        public Animate()
        {
            setup();
            reset();
        }

        public void Go()
        {
            engine.Start();
            startDraw();
            startKeyboardInput();
        }

        private void setup()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(WIDTH, HEIGHT);
            Console.SetBufferSize(WIDTH, HEIGHT);
            
            scoreBoard = new ScoreBoard(0, ARENA_HEIGHT + SCORE_Y_OFFSET);
            messageBoard = new MessageBoard(0, ARENA_HEIGHT + LOG_Y_OFFSET);

            player = new Player(ARENA_WIDTH / 2, ARENA_HEIGHT);

            environment = new Environment(ARENA_WIDTH, ARENA_HEIGHT)
            {
                GameObjects = new List<GameObject>
                    { scoreBoard, messageBoard, player }
            };

            engine = new Engine()
            {
                Environment = environment
            };

            sleepTime = 1000 / engine.FramesPerSecond;

            flip = false;
        }

        private void reset()
        {
            engine.Reset();
            environment.Reset();

            level = INITIAL_LEVEL;
            obstacles = new List<Obstacle>();
            randomObstacleDistribution = new Random().NextDouble() > 0.5;
            removeDeactiveAndGenerateNewObstacles();
        }

        private void startDraw()
        {
            Thread t = new Thread(draw);
            t.Start();
        }

        private void startKeyboardInput()
        {
            var consoleKeyInfo = new ConsoleKeyInfo();

            while (consoleKeyInfo.Key != ConsoleKey.Q)
            {
                if(Console.KeyAvailable)
                {
                    consoleKeyInfo = Console.ReadKey(true);
                }

                if(consoleKeyInfo.Key == ConsoleKey.A)
                {
                    player.MoveLeft();
                } else if(consoleKeyInfo.Key == ConsoleKey.D)
                {
                    player.MoveRight();
                } else if (consoleKeyInfo.Key == ConsoleKey.W)
                {
                    player.MoveUp();
                } else if (consoleKeyInfo.Key == ConsoleKey.S)
                {
                    player.MoveDown();
                } else if (consoleKeyInfo.Key == ConsoleKey.R && 
                    engine.State != Engine.GameState.Running)
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
            while (engine.State == Engine.GameState.Running)
            {
                update();
                render();
                Thread.Sleep(sleepTime);
            }
            messageBoard.Write("Game over. Press 'r' to restart.");
        }

        /// <summary>
        /// Calls functions to draw stuff to the screen
        /// </summary>
        private void render()
        {
            environment.Draw();
            //drawObstacles();
            //player.Draw();
            //scoreBoard.Draw();
        }

        /// <summary>
        /// Calls functions to update the position / state of stuff
        /// </summary>
        private void update()
        {
            if (engine.State == Engine.GameState.Running)
            {
                flip = !flip;
                environment.Update();
                moveObstacles();
                checkObstaclesPosition();
                removeDeactiveAndGenerateNewObstacles();
                scoreBoard.Increment(ARENA_HEIGHT - player.Y);
                if (scoreBoard.Score > (level * LEVEL_THRESHOLD))
                {
                    increaseLevel();
                }
                checkCollisions();
            }
        }

        private void drawObstacles()
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.Draw();
            }
        }

        private void increaseLevel()
        {
            level++;
        }

        private void removeDeactiveAndGenerateNewObstacles()
        {
            if (!randomObstacleDistribution)
            {
                if(obstacles.Count(o => o.State == Obstacle.ObjectState.Deactive) > 0 || obstacles.Count == 0)
                {
                    obstacles.Clear();
                    generateObstacles(level);
                }
            }
            else
            {
                obstacles.RemoveAll(o => o.State == Obstacle.ObjectState.Deactive);

                // always leave one space to move through
                var requiredObstacleCount = Math.Min(level - obstacles.Count, environment.Width - 1);

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
                environment.GameObjects.Add(obstacle);
            }
        }

        private Obstacle generateObstacle()
        {
            var startPosition = new Random().Next(1, ARENA_WIDTH - 1);
            var obstacle = new Obstacle(startPosition, 1);
            obstacle.State = GameObject.ObjectState.Active;

            return obstacle;
        }

        private void checkCollisions()
        {
            foreach (var obstacle in obstacles)
            {
                if (obstacle.X == player.X && obstacle.Y == player.Y)
                {
                    obstacle.State = GameObject.ObjectState.Deactive;
                    engine.State = Engine.GameState.Lost;
                    player.State = GameObject.ObjectState.Deactive;
                }
            }
        }

        private void checkObstaclesPosition()
        {
            foreach (var obstacle in obstacles)
            {
                if (obstacle.Y >= environment.Height)
                {
                    obstacle.State = GameObject.ObjectState.Deactive;
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

    }
}
