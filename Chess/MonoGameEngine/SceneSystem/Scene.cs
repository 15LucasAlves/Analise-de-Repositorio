using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine
{
    abstract class Scene : DrawableAppObject
    {
        private HashSet<AppObject> _sceneObjects = new HashSet<AppObject>();
        private HashSet<ILoadable> _loadables = new HashSet<ILoadable>();
        private HashSet<IUpdatable> _updatables = new HashSet<IUpdatable>();
        private HashSet<IDrawable> _drawables = new HashSet<IDrawable>();


        public Scene() : base()
        {

        }


        /// <summary>
        /// Method all children of Scene must define.
        /// Use app.Content.Load() and AddObjects() calls here to define what is going to be on the scene.
        /// Scene will also automatically call Load() on any ILoadables added to the scene.
        /// </summary>
        protected abstract void OnSceneLoad(MonoGameApp app);


        protected override void OnLoad(MonoGameApp app)
        {
            OnSceneLoad(app);

            foreach (ILoadable loadable in _loadables)
            {
                loadable.Load(app);
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (IUpdatable updatable in _updatables)
            {
                updatable.Update(gameTime);
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            foreach (IDrawable drawable in _drawables)
            {
                drawable.Draw(spriteBatch);
            }
        }


        public void AddObjects(params AppObject[] sceneObjects)
        {
            foreach (AppObject sceneObject in sceneObjects)
            {
                _sceneObjects.Add(sceneObject);

                if (sceneObject is ILoadable)
                {
                    _loadables.Add(sceneObject as ILoadable);
                }
                if (sceneObject is IUpdatable)
                {
                    _updatables.Add(sceneObject as IUpdatable);
                }
                if (sceneObject is IDrawable)
                {
                    _drawables.Add(sceneObject as IDrawable);
                }
            }
        }

        public void RemoveObjects(params AppObject[] sceneObjects)
        {
            foreach (AppObject sceneObject in sceneObjects)
            {
                _sceneObjects.Remove(sceneObject);

                if (sceneObject is ILoadable)
                {
                    _loadables.Remove(sceneObject as ILoadable);
                }
                if (sceneObject is IUpdatable)
                {
                    _updatables.Remove(sceneObject as IUpdatable);
                }
                if (sceneObject is IDrawable)
                {
                    _drawables.Remove(sceneObject as IDrawable);
                }
            }
        }

        public void ClearObjects()
        {
            _sceneObjects.Clear();
            _updatables.Clear();
            _drawables.Clear();
        }
    }
}
