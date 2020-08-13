using Microsoft.Xna.Framework;

namespace Chess
{
    abstract class AppObject
    {
        public Vector2 Position { get; set; }

        public AppObject()
        {
            Position = Vector2.Zero;
        }

        public AppObject(Vector2 position)
        {
            Position = position;
        }
    }
}