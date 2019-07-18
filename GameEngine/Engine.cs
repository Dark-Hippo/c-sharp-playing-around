namespace GameEngine
{
    public class Engine
    {
        public enum GameState
        {
            Won,
            Lost,
            Running,
            Paused
        }

        public GameState State { get; set; }

        public int FramesPerSecond { get; set; } = 10;

        public IEnvironment Environment { get; set; }

        public void Start()
        {
            Environment.Draw();
        }

        public virtual void Reset()
        {
            State = GameState.Running;
        }
    }
}
