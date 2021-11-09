namespace MonoGameEngine.Interfaces
{
    interface IClickable : IUpdatable, IDrawable
    {
        void OnMouseDown();
        void OnMouse();
        void OnMouseUp();
        void OnHoverEnter();
        void OnHover();
        void OnHoverExit();
    }
}
