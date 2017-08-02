namespace GameEngine
{
    public class Obstacle : GameObject
    {
        public enum ObstacleState
        {
            Active,
            Deactive
        }

        public override char Character { get; set; } = '|';

        public ObstacleState State { get; set; }
        
        public Obstacle(int initialX, int initialY) 
            : base(initialX, initialY)
        {
        }
    }
}
