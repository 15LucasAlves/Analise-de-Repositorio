using Microsoft.Xna.Framework;

namespace MonoGameEngine
{
    class GameObject : AppObject
    {
        public string GameObjectName { get; set; }

        public Transform Transform { get; private set; }


        public GameObject() : base()
        {
            GameObjectName = GetType().Name;
            Transform = new Transform(this);
        }


        public override void Update(GameTime gameTime)
        {
            if (!Enabled || gameTime == null)
            {
                return;
            }
            
            OnUpdate(gameTime);

            foreach(var child in Transform.Children)
            {
                child.GameObject.Update(gameTime);
            }
        }
    }
}
