using System.Collections.Generic;
using System.Linq;

namespace Chess.Managers
{
    class SceneManager
    {
        private List<ObjectContainer> scenes;

        public ObjectContainer CurrentScene { get; private set; }

        public SceneManager(IEnumerable<ObjectContainer> scenes)
        {
            this.scenes = scenes.ToList();
            TransitionToScene(scenes.First());
        }

        public void TransitionToScene(ObjectContainer scene)
        {
            if (CurrentScene != null)
                CurrentScene.Active(false);
            CurrentScene = scene;
            scene.Active(true);
        }
    }
}
