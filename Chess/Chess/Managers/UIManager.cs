using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Chess.UI;

namespace Chess.Managers
{
    
    class UIManager
    {
        private UIData uiData;

        private Queue<Action> debugLogCalls;
        private const float debugLinesOffset = 25f;

        private SpriteBatch spriteBatch => uiData.SpriteBatch;

        public UIManager(UIData data)
        {
            this.uiData = data;

            debugLogCalls = new Queue<Action>();
        }

        public Button CreateButton(Rectangle rectangle, string text)
        {
            return new Button(uiData.UITextures[UITextureType.Button], rectangle, uiData.ButtonHoverTint, text, uiData.Fonts[FontType.Button]);
        }

        public Button CreateButton(Rectangle rectangle, string text, Action clickFunction)
        {
            return new Button(uiData.UITextures[UITextureType.Button], rectangle, uiData.ButtonHoverTint, text, uiData.Fonts[FontType.Button], clickFunction);
        }

        public void LogDebugLine(string text)
        {
            debugLogCalls.Enqueue(() => { spriteBatch.DrawString(uiData.Fonts[FontType.Debug], text, new Vector2(0, debugLogCalls.Count * debugLinesOffset), Color.White); });
            
        }

        public void DrawDebug()
        {
            if (AppManager.DebugMode)
            {
                while (debugLogCalls.Count > 0)
                {
                    debugLogCalls.Dequeue().Invoke();
                }
            }
        }
    }
}
