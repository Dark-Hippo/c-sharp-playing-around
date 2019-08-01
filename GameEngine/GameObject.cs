using System;

namespace GameEngine
{
    /// <summary>
    /// A dynamic object within the game environment
    /// </summary>
    public abstract class GameObject
    {
        private readonly int initialX;
        private readonly int initialY;

        public enum ObjectState
        {
            Active,
            Deactive
        }
        
        public ObjectState State { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        
        public abstract char Character { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialX">Initial position on the X axis</param>
        /// <param name="initialY">Initial position on the Y axis</param>
        public GameObject(int initialX, int initialY)
        {
            this.initialX = X = initialX;
            this.initialY = Y = initialY;
        }

        /// <summary>
        /// This function should contain code to 
        /// automatically move game objects according 
        /// to the rules of the game
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// This function should contain code to 
        /// render the game object character to the 
        /// screen in the X, Y position if its state
        /// is set to active
        /// </summary>
        public virtual void Draw()
        {
            if (State == ObjectState.Active)
            {
                Console.SetCursorPosition(X, Y);
                Console.Write(Character);
            }
        }

        /// <summary>
        /// This function should contain code to
        /// reset the game object to it's initial 
        /// state
        /// </summary>
        public virtual void Reset()
        {
            State = ObjectState.Active;
            ResetPosition();
        }

        private void ResetPosition()
        {
            X = initialX;
            Y = initialY;
        }

    }
}
