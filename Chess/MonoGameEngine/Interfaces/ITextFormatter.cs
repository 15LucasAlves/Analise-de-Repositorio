using Microsoft.Xna.Framework;

namespace MonoGameEngine
{
    public enum TextHorizontalAlignment { Left, Center, Right }
    public enum TextVerticalAlignment { Top, Center, Bottom }

    interface ITextFormatter
    {
        TextHorizontalAlignment TextHorizontalAlignment { get; set; }
        TextVerticalAlignment TextVerticalAlignment { get; set; }
        Vector2 TextPadding { get; set; }
    }
}
