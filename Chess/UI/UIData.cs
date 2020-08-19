using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Chess.UI
{
    public enum FontType { Button, Debug, Win }
    public enum UITextureType { Button }

    struct UIData
    {
        public SpriteBatch SpriteBatch;

        #region Fonts

        #endregion

        public Dictionary<FontType, SpriteFont> Fonts;
        public Dictionary<UITextureType, Texture2D> UITextures;
        
        public readonly Color ButtonHoverTint;

        public UIData(SpriteBatch spriteBatch,
                      Dictionary<FontType, SpriteFont> fonts,
                      Dictionary<UITextureType, Texture2D> uiTextures)
        {
            SpriteBatch = spriteBatch;
            Fonts = fonts;
            UITextures = uiTextures;

            ButtonHoverTint = Color.Yellow;
        }
    }
}
