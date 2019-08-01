using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public class Environment : IEnvironment
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public char SideWall { get; set; } = '|';
        public char EndWall { get; set; } = '-';
        public char Floor { get; set; } = ' ';

        public IList<GameObject> GameObjects { get; set; }
        public IList<EnvironmentObject> EnvironmentObjects { get; set; }

        public Environment(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public void Update()
        {
            var obstacles = GameObjects.Where(o => o.GetType() == typeof(Obstacle));
            var player = (Player)GameObjects.Single(o => o.GetType() == typeof(Player));

            UpdateGameObjects();
            UpdateAndCheckObstaclePosition(obstacles);
            CheckPlayerObjectPosition(player);
            IncrementScore(player);
        }

        private void IncrementScore(Player player)
        {
            var scoreBoard = (ScoreBoard)EnvironmentObjects.Single(o => o.GetType() == typeof(ScoreBoard));
            scoreBoard.Increment(Height - player.Y);
        }

        private void UpdateAndCheckObstaclePosition(IEnumerable<GameObject> obstacles)
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.Update();

                if (obstacle.Y >= Height)
                    obstacle.State = GameObject.ObjectState.Deactive;
            }
        }

        private void CheckPlayerObjectPosition(Player player)
        {
            if (player.X > Width)
                player.X = Width;
            else if (player.X <= 1)
                player.X = 1;
            else if (player.Y > Height)
                player.Y = Height;
            else if (player.Y <= 1)
                player.Y = 1;
        }

        private void UpdateGameObjects()
        {
            var objects = new GameObject[GameObjects.Count];
            GameObjects.CopyTo(objects, 0);
            foreach (var item in objects)
            {
                if (item.State == GameObject.ObjectState.Deactive)
                {
                    GameObjects.Remove(item);
                }
            }
        }

        public void Draw()
        {
            DrawEnvironment();
            DrawGameObjects();
            DrawEnvironmentObjects();
        }

        public void Reset()
        {
            foreach (var item in GameObjects)
            {
                item.Reset();
            }

            foreach (var item in EnvironmentObjects)
            {
                item.Reset();
            }
        }
        
        private void DrawEnvironment()
        {
            bool flip = false;
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i <= this.Height + 1; i++)
            {
                for (int j = 0; j <= this.Width + 1; j++)
                {
                    if (i == 0 || i == this.Height + 1)
                    {
                        Console.Write(this.EndWall);
                    }
                    else if (j == 0 || j == this.Width + 1)
                    {
                        if (flip)
                            Console.Write(this.SideWall);
                        else
                            Console.Write(this.Floor);
                    }
                    else
                    {
                        Console.Write(this.Floor);
                    }
                }
                Console.WriteLine();
                flip = !flip;
            }
        }

        private void DrawGameObjects()
        {
            foreach (var item in GameObjects)
            {
                item.Draw();
            }
        }

        private void DrawEnvironmentObjects()
        {
            foreach (var item in EnvironmentObjects)
            {
                item.Draw();
            }
        }
    }
}
