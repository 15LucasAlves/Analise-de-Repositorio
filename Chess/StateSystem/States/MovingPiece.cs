using Microsoft.Xna.Framework;

namespace Chess
{
    class MovingPiece : ChessGameManagerState
    {
        private Color SelectedPieceTint => Color.LightSkyBlue;
        private Color SelectedPiecePossibleMovesTint => Color.Yellow;

        public override void Enter()
        {
            // Get the filtered possible moves of the selected piece
            GameManager.SelectedPiecePossibleMoves = GameManager.GetFilteredPossibleMoves(GameManager.SelectedPiece);

            // Tint everything properly
            GameManager.SelectedPiece.TilePosition.Tint = SelectedPieceTint;

            foreach (Tile tile in GameManager.SelectedPiecePossibleMoves)
            {
                tile.Tint = SelectedPiecePossibleMovesTint;
            }
        }

        public override void Exit()
        {
            // Clear tints
            GameManager.SelectedPiece.TilePosition.Tint = Color.White;
            foreach (Tile tile in GameManager.SelectedPiecePossibleMoves)
            {
                tile.Tint = Color.White;
            }

            // Set selected piece pointer to null
            GameManager.SelectedPiece = null;

            // Set selected piece possible moves pointer to null
            GameManager.SelectedPiecePossibleMoves = null;
        }

        public override void Update(GameTime gameTime)
        {
            if (MonoGameEngine.Mouse.IsRightMouseDown)
            {
                GameManager.DeselectSelectedPiece();
            }
        }
    }
}
