using System;

namespace GameEngine
{
    public class Player : GameObject
    {
        public override char Character { get; set; } = 'A';
        public char Explosion { get; set; } = 'x';

        private readonly IEnvironment environment;

        public Player(int initialX, int initialY, 
            IEnvironment environment) 
            : base(initialX, initialY)
        {
            this.environment = environment;
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

        public void MoveLeft()
        {
            if (X > 1) X--;
        }

        public void MoveRight()
        {
            if (X < environment.Width) X++;
        }

        public void MoveUp()
        {
            if (Y > 1) Y--;
        }

        public void MoveDown()
        {
            if (Y < environment.Height) Y++;
        }
    }
}