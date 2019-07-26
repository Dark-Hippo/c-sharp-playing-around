﻿using System;
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

        public Environment(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public void Update()
        {
            var objects = new GameObject[GameObjects.Count];
            GameObjects.CopyTo(objects, 0);
            foreach (var item in objects)
            {
                if(item.State == GameObject.ObjectState.Deactive)
                {
                    GameObjects.Remove(item);
                }
            }
        }

        public void Draw()
        {
            DrawEnvironment();
            DrawGameObjects();

            MessageBoard messageBoard = GameObjects.Single(x => x.GetType() == typeof(MessageBoard)) as MessageBoard;
            messageBoard.Reset();
            messageBoard.Write($"Currently {GameObjects.Count()} objects");
        }

        public void Reset()
        {
            foreach (var item in GameObjects)
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
    }
}
