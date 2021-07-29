using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    class ObjectContainer : AppObject, Interfaces.IDrawable, Interfaces.IUpdatable, IEnumerable
    {
        // Whether objects inside will be drawn
        public bool Visible { get; set; }
        // Whether objects inside will be updated
        public bool Enabled { get; set; }
        public int Count => containerObjects.Count;
        public bool IsEmpty => Count <= 0;

        private List<AppObject> containerObjects;
        private List<Interfaces.IDrawable> drawables;
        private List<Interfaces.IUpdatable> updatables;

        public IEnumerator GetEnumerator()
        {
            return new ContainerEnumerator(this);
        }

        private class ContainerEnumerator : IEnumerator
        {
            private ObjectContainer instance;
            private int index = -1;
            public object Current => instance.containerObjects[index];

            public ContainerEnumerator(ObjectContainer instance)
            {
                this.instance = instance;
            }

            public bool MoveNext()
            {
                index++;
                return (index < instance.containerObjects.Count);
            }

            public void Reset()
            {
                index = 0;
            }
        } 

        public ObjectContainer()
        {
            containerObjects = new List<AppObject>();
            drawables = new List<Interfaces.IDrawable>();
            updatables = new List<Interfaces.IUpdatable>();
        }

        // Enables/Disables both drawing and updating
        public void Active(bool activeState)
        {
            Visible = activeState;
            Enabled = activeState;
        }

        public void AddChild(params AppObject[] containerObjects)
        {
            foreach (AppObject containerObject in containerObjects)
            {
                this.containerObjects.Add(containerObject);
                if (containerObject is Interfaces.IDrawable)
                    drawables.Add(containerObject as Interfaces.IDrawable);
                if (containerObject is Interfaces.IUpdatable)
                    updatables.Add(containerObject as Interfaces.IUpdatable);
            }
        }

        public void AddChild(IEnumerable<AppObject> containerObjects)
        {
            this.containerObjects.AddRange(containerObjects);
            drawables.AddRange(containerObjects.OfType<Interfaces.IDrawable>());
            updatables.AddRange(containerObjects.OfType<Interfaces.IUpdatable>());
        }

        public bool RemoveChild(AppObject containerObject)
        {
            if (containerObjects.Remove(containerObject))
            {
                if (containerObject is Interfaces.IDrawable)
                    drawables.Remove(containerObject as Interfaces.IDrawable);
                if (containerObject is Interfaces.IUpdatable)
                    updatables.Remove(containerObject as Interfaces.IUpdatable);
                return true;
            }

            return false;
        }

        public void RemoveChildren(IEnumerable<AppObject> containerObjects)
        {
            this.containerObjects.RemoveRange(this.containerObjects.IndexOf(containerObjects.First()), containerObjects.Count());
            drawables.RemoveRange(drawables.IndexOf(containerObjects.OfType<Interfaces.IDrawable>().First()), containerObjects.OfType<Interfaces.IDrawable>().Count());
            updatables.RemoveRange(updatables.IndexOf(containerObjects.OfType<Interfaces.IUpdatable>().First()), containerObjects.OfType<Interfaces.IUpdatable>().Count());
        }

        public void Clear()
        {
            containerObjects.Clear();
            drawables.Clear();
            updatables.Clear();
        }

        public List<AppObject> ToList()
        {
            return containerObjects;
        }

        public void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                for (int i = 0; i < updatables.Count; i++)
                {
                    updatables[i].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                for (int i = 0; i < drawables.Count; i++)
                {
                    drawables[i].Draw(spriteBatch);
                }
            }
        }
    }
}
