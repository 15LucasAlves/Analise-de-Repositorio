using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameEngine
{
    class Transform
    {
        // Back reference to the GameObject this Transform belongs to.
        public GameObject GameObject { get; private set; }


        public Transform(GameObject gameObject)
        {
            GameObject = gameObject;
        }


        private Vector3 _globalPosition;
        public Vector3 GlobalPosition
        {
            get => _globalPosition;
            private set
            {
                // Clamp Z between 0 and 1 to avoid silent error caused by layerDepth being out of range
                Vector3 clampedPosition = new Vector3(value.X, value.Y, MathHelper.Clamp(value.Z, 0f, 1f));

                // Update position
                _globalPosition = clampedPosition;

                // Update children's global position
                foreach (Transform child in _children)
                {
                    child.GlobalPosition = _globalPosition + Vector3.Transform(new Vector3(child._position.X * _globalScale.X, child._position.Y * _globalScale.Y, child._position.Z), Matrix.CreateRotationZ(_globalRotation));
                }
            }
        }

        private Vector3 _position;
        public Vector3 Position
        {
            get => _position;
            set
            {
                // Clamp Z between 0 and 1 to avoid silent error caused by layerDepth being out of range
                Vector3 clampedPosition = new Vector3(value.X, value.Y, MathHelper.Clamp(value.Z, 0f, 1f));

                // Cache displacement to update global position later
                Vector3 displacement = clampedPosition - _position;

                // Update position
                _position = clampedPosition;

                // Update global position
                _globalPosition += displacement;

                // Update children's global position
                foreach (Transform child in _children)
                {
                    child.GlobalPosition = _globalPosition + Vector3.Transform(new Vector3(child._position.X * _globalScale.X, child._position.Y * _globalScale.Y, child._position.Z), Matrix.CreateRotationZ(_globalRotation));
                }
            }
        }

        private float _globalRotation;
        public float GlobalRotation
        {
            get => _globalRotation;
            private set
            {
                // Update global rotation
                _globalRotation = value;

                // Update children's global rotation
                foreach (Transform child in _children)
                {
                    child.GlobalRotation = _globalRotation + child._rotation;
                }
            }
        }

        private float _rotation;
        public float Rotation
        {
            get => _rotation;
            set
            {
                // Cache delta rotation to update global rotation later
                float deltaRotation = value - _rotation;

                // Update rotation
                _rotation = value;

                // Update global rotation
                _globalRotation += deltaRotation;

                // Update children's global rotation
                foreach (Transform child in _children)
                {
                    child.GlobalRotation = _globalRotation + child._rotation;
                }
            }
        }

        private Vector2 _origin = Vector2.Zero;
        public Vector2 Origin
        {
            get => _origin;
            set
            {
                // Update origin
                _origin = value;
            }
        }

        private Vector2 _globalScale = Vector2.One;
        public Vector2 GlobalScale
        {
            get => _globalScale;
            private set
            {
                // Update global scale
                _globalScale = value;

                // Update children's global scale
                foreach (Transform child in _children)
                {
                    child.GlobalScale = _globalScale * child._scale;
                }
            }
        }

        private Vector2 _scale = Vector2.One;
        public Vector2 Scale
        {
            get => _scale;
            set
            {
                // Cache delta scale to update global scale later
                Vector2 deltaScale = value - _scale;

                // Update scale
                _scale = value;

                // Update global scale
                _globalScale += deltaScale;

                // Update children's global position
                foreach (Transform child in _children)
                {
                    child.GlobalScale = _globalScale * child._scale;
                }
            }
        }


        // Parenting code
        private Transform _parent;
        public Transform Parent
        {
            get => _parent;
            set
            {
                if (_parent != null)
                {
                    // Remove from the old parent's children
                    _parent._children.Remove(this);
                }

                // Set new parent
                _parent = value;

                // Add this to the new parent's children
                _parent._children.Add(this);

                // Update Parameters after changing parent
                GlobalPosition = GlobalPosition;
                GlobalRotation = GlobalRotation;
                GlobalScale = GlobalScale;
            }
        }

        private HashSet<Transform> _children = new HashSet<Transform>();
        public IEnumerable<Transform> Children => _children;

        public void AddChildren(params Transform[] children)
        {
            foreach (Transform child in children)
            {
                if (child._parent != null)
                {
                    child._parent._children.Remove(child);
                }

                child._parent = this;
                _children.Add(child);
            }
        }

        public void AddChildren(params GameObject[] children)
        {
            foreach (GameObject gameObject in children)
            {
                Transform child = gameObject.Transform;

                if (child._parent != null)
                {
                    child._parent._children.Remove(child);
                }

                child._parent = this;
                _children.Add(child);
            }
        }

        public void RemoveChildren(params Transform[] children)
        {
            foreach (Transform child in children)
            {
                if (_children.Contains(child))
                {
                    child._parent = null;
                    _children.Remove(child);
                }
            }
        }

        public void RemoveChildren(params GameObject[] children)
        {
            foreach (GameObject gameObject in children)
            {
                Transform child = gameObject.Transform;

                if (_children.Contains(child))
                {
                    child._parent = null;
                    _children.Remove(child);
                }
            }
        }

        public void ClearChildren()
        {
            foreach (Transform child in _children)
            {
                child._parent = null;
                _children.Remove(child);
            }
        }
    }
}
