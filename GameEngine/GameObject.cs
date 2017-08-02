namespace GameEngine
{
    public abstract class GameObject
    {
        private readonly int initialX;
        private readonly int initialY;

        public int X { get; set; }
        public int Y { get; set; }
        
        public abstract char Character { get; set; }

        public GameObject(int initialX, int initialY)
        {
            this.initialX = X = initialX;
            this.initialY = Y = initialY;
        }

        public void ResetPosition()
        {
            X = initialX;
            Y = initialY;
        }
    }
}
