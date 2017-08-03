using System;

namespace GameEngine
{
    public class Obstacle : GameObject
    {
        public override char Character { get; set; } = '|';
        
        public Obstacle(int initialX, int initialY) 
            : base(initialX, initialY)
        {
        }

        public override void Update()
        {
            this.Y++;
        }
    }
}
