using System.Collections.Generic;

namespace Chess
{
    class UndoRedoStack<T>
    {
        private Stack<T> _backStack = new Stack<T>();
        private Stack<T> _frontStack = new Stack<T>();

        public UndoRedoStack()
        {
        }

        public IEnumerable<T> BackCollection()
        {
            return _backStack;
        }

        public IEnumerable<T> FrontCollection()
        {
            return _frontStack;
        }

        public void Push(T item)
        {
            _frontStack.Clear();

            _backStack.Push(item);
        }

        public T BackPop()
        {
            if (_backStack.Count > 0)
            {
                T undoItem = _backStack.Pop();
                _frontStack.Push(undoItem);
                return undoItem;
            }

            return default(T);
        }

        public T FrontPop()
        {
            if (_frontStack.Count > 0)
            {
                T redoItem = _frontStack.Pop();
                _backStack.Push(redoItem);
                return redoItem;
            }

            return default(T);
        }

        public void Clear()
        {
            _backStack.Clear();
            _frontStack.Clear();
        }
    }
}
