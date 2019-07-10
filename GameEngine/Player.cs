using System;

namespace GameEngine
{
    public class Player : GameObject
    {
        public override char Character { get; set; } = '^';
        public char Explosion { get; set; } = '.';

        public Player(int initialX, int initialY) 
            : base(initialX, initialY)
        {
        }

        public override void Update()
        {
            // player position is not updated automatically
        }

        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            if (State == ObjectState.Active)
                Console.Write(Character);
            else
                MakeGoBang();
        }

        private void MakeGoBang()
        {
            for (int x = X - 1; x <= X + 1; x++)
            {
                for (int y = Y - 1; y <= Y + 1; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(Explosion);
                }
            }

        }
    }
}