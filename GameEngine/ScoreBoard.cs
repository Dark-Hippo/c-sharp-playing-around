using System;

namespace GameEngine
{
    public class ScoreBoard : EnvironmentObject
    {
        public int Score { private set; get; } = 0;

        public ScoreBoard(int initialX, int initialY) 
            : base(initialX, initialY)
        {
        }

        public override char Character { get; set; } = ' ';

        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write("Score: " + Score);
        }

        public void Increment(int value)
        {
            Score += value;
        }

        public override void Reset()
        {
            Score = 0;
            Console.SetCursorPosition(X, Y);
            Console.Write(new string(Character, Console.WindowWidth));
        }
    }
}
