using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class Environment : IEnvironment
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public char SideWall { get; set; } = '|';
        public char EndWall { get; set; } = '-';
        public char Floor { get; set; } = ' ';

        public IEnumerable<GameObject> GameObjects { get; set; }

        public Environment(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public void Draw()
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

        public void Reset()
        {
            foreach (var item in GameObjects)
            {
                item.Reset();
            }
        }
    }
}
