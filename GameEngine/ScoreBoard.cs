using System;

namespace GameEngine
{
    public class ScoreBoard : GameObject
    {
        public int Score { private set; get; } = 0;

        public ScoreBoard(int initialX, int initialY) 
            : base(initialX, initialY)
        {
        }

        public override char Character { get; set; } = ' ';

        public override void Update()
        {
            // position never changes as part of the game
        }

        public override void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write("Score: " + Score);
        }

        public void Clear()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(new string(Character, Console.WindowWidth));
        }

        public void Increment(int value)
        {
            Score += value;
        }

        public void Reset()
        {
            Score = 0;
        }
    }
}
