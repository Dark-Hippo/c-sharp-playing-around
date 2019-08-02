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
        private bool randomObstacleDistribution;

        public Animate()
        {
            Setup();
            Reset();
        }

        public void Go()
        {
            engine.Start();
            StartDraw();
            StartKeyboardInput();
        }

        private void Setup()
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
                    { player },
                EnvironmentObjects = new List<EnvironmentObject>
                    { scoreBoard, messageBoard }
            };

            engine = new Engine()
            {
                Environment = environment
            };

            sleepTime = 1000 / engine.FramesPerSecond;
        }

        private void Reset()
        {
            engine.Reset();
            environment.Reset();

            level = INITIAL_LEVEL;
            obstacles = new List<Obstacle>();
            randomObstacleDistribution = new Random().NextDouble() > 0.5;
            RemoveDeactiveAndGenerateNewObstacles();
        }

        private void StartDraw()
        {
            Thread t = new Thread(Draw);
            t.Start();
        }

        private void StartKeyboardInput()
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
                    Reset();
                    Go();
                }
                consoleKeyInfo = default;
            }
        }
        
        /// <summary>
        /// Main game loop
        /// </summary>
        private void Draw()
        {
            while (engine.State == Engine.GameState.Running)
            {
                Update();
                environment.Draw();
                Thread.Sleep(sleepTime);
            }
            messageBoard.Write("Game over. Press 'r' to restart.");
        }
        
        /// <summary>
        /// Calls functions to update the position / state of stuff
        /// </summary>
        private void Update()
        {
            if (engine.State == Engine.GameState.Running)
            {
                environment.Update();
                RemoveDeactiveAndGenerateNewObstacles();
                if (scoreBoard.Score > (level * LEVEL_THRESHOLD))
                {
                    IncreaseLevel();
                }
                CheckCollisions();
            }
        }

        private void IncreaseLevel()
        {
            level++;
        }

        private void RemoveDeactiveAndGenerateNewObstacles()
        {
            if (!randomObstacleDistribution)
            {
                if(obstacles.Count(o => o.State == GameObject.ObjectState.Deactive) > 0 || obstacles.Count == 0)
                {
                    obstacles.Clear();
                    GenerateObstacles(level);
                }
            }
            else
            {
                obstacles.RemoveAll(o => o.State == GameObject.ObjectState.Deactive);

                // always leave one space to move through
                var requiredObstacleCount = Math.Min(level - obstacles.Count, environment.Width - 1);

                GenerateObstacles(requiredObstacleCount);
            }
        }

        private void GenerateObstacles(int requiredObstacleCount)
        {
            for (int i = 0; i < requiredObstacleCount; i++)
            {
                // this will doom loop if all obstacle positions are taken.
                var obstacle = GenerateObstacle();
                while (obstacles.Count(o => o.X == obstacle.X) != 0)
                {
                    obstacle = GenerateObstacle();
                }
                obstacles.Add(obstacle);
                environment.GameObjects.Add(obstacle);
            }
        }

        private Obstacle GenerateObstacle()
        {
            var startPosition = new Random().Next(1, ARENA_WIDTH);
            var obstacle = new Obstacle(startPosition, 1)
            {
                State = GameObject.ObjectState.Active
            };

            return obstacle;
        }

        private void CheckCollisions()
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
    }
}
