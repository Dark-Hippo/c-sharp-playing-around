using System;

namespace GameEngine
{
    public class MessageBoard : GameObject
    {
        private string message;

        public MessageBoard(int initialX, int initialY) 
            : base(initialX, initialY)
        {
        }

        public override char Character { get; set; } = ' ';

        public override void Update()
        {
            // position never changes as part of the game
        }

        public void Write(string message)
        {
            this.message = message;
            Draw();
        }

        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(message);
        }

        public override void Reset()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(new string(Character, Console.WindowWidth));
        }
    }
}
