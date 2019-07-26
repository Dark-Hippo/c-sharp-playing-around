using System.Collections.Generic;

namespace GameEngine
{
    public interface IEnvironment
    {
        /// <summary>
        /// Character for the side wall
        /// </summary>
        char SideWall { get; set; }

        /// <summary>
        /// Character for the end wall
        /// </summary>
        char EndWall { get; set; }

        /// <summary>
        /// Character for the floor
        /// </summary>
        char Floor { get; set; }

        /// <summary>
        /// Height of the play area
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Width of the play area
        /// </summary>
        int Width { get; }

        /// <summary>
        /// List of game objects that exist
        /// within the game environment
        /// </summary>
        IList<GameObject> GameObjects { get; set; }

        /// <summary>
        /// Draws the play area and attached 
        /// game objects
        /// </summary>
        void Draw();

        /// <summary>
        /// Resets the environment to its
        /// initial state
        /// </summary>
        void Reset();
    }
}