using System;

namespace GameEngine
{
    public class MessageBoard : EnvironmentObject
    {
        private string message;

        public MessageBoard(int initialX, int initialY) 
            : base(initialX, initialY)
        {
        }

        public override char Character { get; set; } = ' ';

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
