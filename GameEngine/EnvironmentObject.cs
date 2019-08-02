using System;

namespace GameEngine
{
    public abstract class EnvironmentObject
    {
        public readonly int X;
        public readonly int Y;

        public abstract char Character { get; set; }

        /// <summary>
        /// Static environment object
        /// </summary>
        /// <param name="X">Position on the X axis</param>
        /// <param name="Y">Position on the Y axis</param>
        public EnvironmentObject(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public virtual void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(Character);
        }

        public abstract void Reset();
    }
}
