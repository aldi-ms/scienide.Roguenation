using System.Collections;

namespace scienide.Engine.Infrastructure;

/// <summary>
/// А two-dimensional array facade with a single-dimensional array implementation for performance purposes.
/// </summary>
/// <typeparam name="T"></typeparam>
public class FlatArray<T> : ICollection<T>, IEnumerable<T>
{
    private readonly T[] _data;

    public FlatArray(int width, int height)
    {
        Width = width;
        Height = height;
        _data = new T[Width * Height];
    }

    public int Width { get; }

    public int Height { get; }

    public T this[int x, int y]
    {
        get
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _data[x + y * Width];
        }
        set
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                throw new ArgumentOutOfRangeException();
            }

            _data[x + y * Width] = value;
        }
    }

    public int Count => _data.Length;

    public bool IsSynchronized => false;

    public bool IsReadOnly => false;

    public void Clear()
    {
        Array.Clear(_data, 0, _data.Length);
    }

    public bool Contains(T item)
    {
        return Array.IndexOf(_data, item) >= 0;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _data.CopyTo(array, arrayIndex);
    }

    //public void ParallelProcess(Action<T> action)
    //{
    //    Parallel.For(0, _data.Length, i =>
    //    {
    //        action(_data[i]);
    //    });
    //}

    public Span<T> AsSpan()
    {
        return _data.AsSpan();
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Unsupported.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="NotSupportedException"></exception>
    public void Add(T item)
    {
        throw new NotSupportedException("Cannot add an item to a fixed-size array.");
    }

    /// <summary>
    /// Unsupported.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public bool Remove(T item)
    {
        throw new NotSupportedException("Cannot remove an item from a fixed-size array.");
    }

    public struct Enumerator : IEnumerator<T>
    {
        private readonly FlatArray<T> _array;
        private int _index;

        public Enumerator(FlatArray<T> array)
        {
            _array = array;
            _index = -1;
        }

        public T Current => _array._data[_index];

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _index++;
            return _index < _array._data.Length;
        }

        public void Reset()
        {
            _index = -1;
        }

        public void Dispose() { }
    }
}
