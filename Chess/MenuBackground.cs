using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess
{
    class MenuBackground : GameObject, Interfaces.IUpdatable
    {
        private readonly Rectangle[,] startingPositions;
        private Rectangle[,] positions;

        private float elapsedTime = 0f;
        const float delay = 15f;

        public MenuBackground(Texture2D Sprite, Rectangle rectangle) : base(Sprite, rectangle)
        {
            startingPositions = new Rectangle[2, 2];
            positions = new Rectangle[2, 2];

            startingPositions[0, 0] = new Rectangle(rectangle.X + Sprite.Width * -1, rectangle.Y, Sprite.Width, Sprite.Height);
            startingPositions[1, 0] = new Rectangle(rectangle.X, rectangle.Y, Sprite.Width, Sprite.Height);
            startingPositions[0, 1] = new Rectangle(rectangle.X + Sprite.Width * -1, rectangle.Y + Sprite.Height, Sprite.Width, Sprite.Height);
            startingPositions[1, 1] = new Rectangle(rectangle.X, rectangle.Y + Sprite.Height, Sprite.Width, Sprite.Height);

            ResetToStartingPosition();
        }

        private void ResetToStartingPosition()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    positions[j, i] = startingPositions[j, i];
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= delay)
            {
                if (positions[0, 1].Y == 0)
                {
                    ResetToStartingPosition();
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        positions[j, i] = new Rectangle(positions[j, 1].X + 1, positions[j, i].Y - 1, positions[j, i].Width, positions[j, i].Height);
                    }
                }
                elapsedTime = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                foreach (Rectangle rect in positions)
                {
                    spriteBatch.Draw(Sprite, rect, Color.White);
                }
            }
        }
    }
}
