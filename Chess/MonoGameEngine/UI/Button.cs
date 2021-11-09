using Microsoft.Xna.Framework;

namespace MonoGameEngine
{
    class Button : ClickableGameObject, ITextFormatter
    {
        public TextGameObject TextGameObject { get; private set; } = new TextGameObject();

        private TextHorizontalAlignment _textHorizontalAlignment = TextHorizontalAlignment.Left;
        public TextHorizontalAlignment TextHorizontalAlignment
        {
            get => _textHorizontalAlignment;
            set
            {
                _textHorizontalAlignment = value;

                TextGameObject.Transform.Position = new Vector3(GetTextPositionX(), TextGameObject.Transform.Position.Y, TextGameObject.Transform.Position.Z);
            }
        }

        private TextVerticalAlignment _textVerticalAlignment = TextVerticalAlignment.Top;
        public TextVerticalAlignment TextVerticalAlignment
        {
            get => _textVerticalAlignment;
            set
            {
                _textVerticalAlignment = value;

                TextGameObject.Transform.Position = new Vector3(TextGameObject.Transform.Position.X, GetTextPositionY(), TextGameObject.Transform.Position.Z);
            }
        }

        private Vector2 _textPadding;
        public Vector2 TextPadding
        {
            get => _textPadding;
            set
            {
                _textPadding = value;

                TextGameObject.Transform.Position = new Vector3(GetTextPositionX(), GetTextPositionY(), TextGameObject.Transform.Position.Z);
            }
        }


        public Button() : base()
        {
            Transform.AddChildren(TextGameObject.Transform);
        }


        // Helper functions that determine TextGameObject position based on alignment and padding
        private float GetTextPositionX()
        {
            float TextFontWidth = TextGameObject.Font.MeasureString(TextGameObject.Text).X;

            switch (_textHorizontalAlignment)
            {
                case TextHorizontalAlignment.Left:
                    return TextPadding.X;
                case TextHorizontalAlignment.Center:
                    return Texture.Width * Transform.GlobalScale.X * 0.5f - TextFontWidth * 0.5f;
                case TextHorizontalAlignment.Right:
                    return Texture.Width * Transform.GlobalScale.X - TextPadding.X - TextFontWidth;
                default:
                    throw new System.Exception("Undefined enum value reached.");
            }
        }

        private float GetTextPositionY()
        {
            float TextFontHeight = TextGameObject.Font.MeasureString(TextGameObject.Text).Y;

            switch (_textVerticalAlignment)
            {
                case TextVerticalAlignment.Top:
                    return -TextPadding.Y;
                case TextVerticalAlignment.Center:
                    return Texture.Height * Transform.GlobalScale.Y * 0.5f - TextFontHeight * 0.5f;
                case TextVerticalAlignment.Bottom:
                    return Texture.Height * Transform.GlobalScale.Y + TextPadding.Y - TextFontHeight;
                default:
                    throw new System.Exception("Undefined enum value reached.");
            }
        }
    }
}
