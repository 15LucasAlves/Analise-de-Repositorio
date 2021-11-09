using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGameEngine;

namespace Chess
{
    class ChessButtonCreator : ILoadable
    {
        public static Texture2D ButtonTexture { get; set; }

        public static SpriteFont ButtonFont { get; set; }

        public static Color ButtonHoverTint { get; set; }

        public void Load(MonoGameApp app)
        {
            ButtonTexture = app.Content.Load<Texture2D>("buttonTexture");
            ButtonFont = app.Content.Load<SpriteFont>("Fonts/buttonFont");

            ButtonHoverTint = Color.Yellow;
        }

        public Button CreateButton(string text = "", Vector3 position = default(Vector3))
        {
            Button button = new Button();
            
            button.Texture = ButtonTexture;
            button.TextGameObject.Font = ButtonFont;
            button.TextGameObject.Text = text;

            button.Transform.Position = position;

            button.TextHorizontalAlignment = TextHorizontalAlignment.Center;
            button.TextVerticalAlignment = TextVerticalAlignment.Center;

            button.TintOnHover = true;
            button.HoverTint = ButtonHoverTint;

            return button;
        }
    }
}
