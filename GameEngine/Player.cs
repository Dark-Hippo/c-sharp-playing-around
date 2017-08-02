﻿namespace GameEngine
{
    public class Player : GameObject
    {
        public override char Character { get; set; } = '^';
        public Player(int initialX, int initialY) 
            : base(initialX, initialY)
        {
        }
    }
}