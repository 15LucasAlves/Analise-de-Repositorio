using Microsoft.Xna.Framework;

static class Collision
{
    public static bool AreColliding(Rectangle a, Rectangle b)
    {
        return a.X < b.X + b.Width &&
               a.X + a.Width > b.X &&
               a.Y < b.Y + b.Height &&
               a.Y + a.Height > b.Y;
    }

    public static bool AreColliding(Rectangle rectangle, Point point)
    {
        return point.X > rectangle.X &&
               point.X < rectangle.X + rectangle.Width &&
               point.Y > rectangle.Y &&
               point.Y < rectangle.Y + rectangle.Height;
    }
}
