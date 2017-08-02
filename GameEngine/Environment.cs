namespace GameEngine
{
    public class Environment
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public char SideWall { get; set; } = '|';
        public char EndWall { get; set; } = '-';
        public char Floor { get; set; } = ' ';

        public Environment(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}
