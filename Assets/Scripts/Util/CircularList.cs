using System.Collections.Generic;


namespace Ventura.Util
{
    public class CircularList<T>
    {
        private List<T> _data;
        private int _pos = -1;


        public CircularList()
        {
            _data = new List<T>();
        }


        public void Add(T item)
        {
            _data.Add(item);
        }

        public void Remove(T targetItem)
        {
            if (_data.Count == 0)
                return;

            var targetPos = _data.IndexOf(targetItem);
            if (targetPos == -1)
                return;

            if (targetPos <= _pos)
                _pos--;

            _data.RemoveAt(targetPos);
        }


        public T? Next()
        {
            if (_data.Count == 0)
                return default(T);

            _pos++;
            _pos %= _data.Count;

            return _data[_pos];
        }

        public void Clear()
        {
            _data.Clear();
            _pos = -1;
        }
    }
}
