using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Sprite : GameObject, IDrawable
{
    // Positioning fields
    public Point Position { get; set; }
    public float Rotation { get; set; }

    // Drawing fields
    public bool Visible { get; set; }
    private Texture2D texture;
    public Texture2D Texture
    {
        get => texture;
        set
        {
            texture = value;
            if (texture == null)
            {
                Visible = false;
            }
            else
            {
                sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            }
        }
    }
    public Point Origin { get; set; }
    public Point Scale { get; set; }
    public float LayerDepth { get; set; }
    public Color Tint { get; set; }
    public SpriteEffects SpriteEffects { get; set; }

    private Rectangle sourceRectangle;

    public Sprite(Point position = default, float rotation = 0f, Texture2D texture = null, Point origin = default, float layerDepth = 0f, Color tint = default, SpriteEffects spriteEffects = default) : base()
    {
        Position = position;
        Rotation = rotation;

        Visible = texture != null;
        Texture = texture;
        Origin = origin;
        Scale = new Point(1, 1);

        LayerDepth = layerDepth;
        Tint = tint;
        SpriteEffects = spriteEffects;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (!Visible) return;
        
        foreach (GameObject child in Children)
        {
            if (child is IDrawable)
            {
                (child as IDrawable).Draw(spriteBatch);
            }
        }

        spriteBatch.Draw(Texture, Position.ToVector2(), sourceRectangle, Tint, Rotation, Origin.ToVector2(), Scale.ToVector2(), SpriteEffects, LayerDepth);
    }
}