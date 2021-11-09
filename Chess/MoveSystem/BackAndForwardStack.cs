using System.Collections.Generic;

namespace Chess
{
    class BackAndForwardStack<T>
    {
        private Stack<T> _backStack = new Stack<T>();
        private Stack<T> _frontStack = new Stack<T>();

        public BackAndForwardStack()
        {
        }

        public void Push(T item)
        {
            _frontStack.Clear();

            _backStack.Push(item);
        }

        public T Back()
        {
            T undoItem = _backStack.Pop();
            _frontStack.Push(undoItem);
            return undoItem;
        }

        public T Forward()
        {
            T redoItem = _frontStack.Pop();
            _backStack.Push(redoItem);
            return redoItem;
        }

        public void Clear()
        {
            _backStack.Clear();
            _frontStack.Clear();
        }
    }
}
