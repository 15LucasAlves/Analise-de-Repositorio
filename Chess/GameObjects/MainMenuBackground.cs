using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGameEngine;

namespace Chess
{
    class MainMenuBackground : TexturedGameObject
    {
        public float MoveSpeed { get; set; } = 64.0F;


        public MainMenuBackground() : base()
        {

        }


        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            // Translation logic
            Transform.Position += new Vector3(MoveSpeed, -MoveSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Wrapping logic
            if (Math.Abs(Transform.Position.X) > Texture.Width * Transform.Scale.X)
            {
                Transform.Position += new Vector3(-Texture.Width * Transform.Scale.X * 2f, 0, 0);
            }
            if (Math.Abs(Transform.Position.Y) > Texture.Height * Transform.Scale.Y)
            {
                Transform.Position += new Vector3(0, Texture.Height * Transform.Scale.Y * 2f, 0);
            }
        }

        protected override void OnLoad(MonoGameApp app)
        {
            base.OnLoad(app);

            Texture = app.Content.Load<Texture2D>("chessBoard");
        }
    }
}
