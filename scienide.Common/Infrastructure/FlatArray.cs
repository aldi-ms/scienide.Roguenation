﻿namespace scienide.Common.Infrastructure;

using SadRogue.Primitives;
using System.Collections;
using System.Collections.ObjectModel;

/// <summary>
/// А two-dimensional array facade with a single-dimensional array implementation for performance purposes.
/// </summary>
/// <typeparam name="T"></typeparam>
public class FlatArray<T> : ICollection<T>, IEnumerable<T>
{
    public readonly int Width;
    public readonly int Height;

    private readonly T[] _data;

    public FlatArray(int width, int height)
        : this(width, height, new T[width * height])
    {
    }

    public FlatArray(int width, int height, T[] data)
    {
        if (data.Length != width * height)
        {
            throw new ArgumentOutOfRangeException(nameof(data));
        }

        Width = width;
        Height = height;
        _data = data;
    }

    public T this[Point pos]
    {
        get => this[pos.X, pos.Y];
        set => this[pos.X, pos.Y] = value;
    }

    public T this[int x, int y]
    {
        get => _data[x + y * Width];
        set => _data[x + y * Width] = value;
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

    public Span<T> AsSpan() => _data.AsSpan();

    public ReadOnlyCollection<T> AsReadOnly() => _data.AsReadOnly();

    public Enumerator GetEnumerator() => new(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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

        public readonly T Current => _array._data[_index];

        readonly object IEnumerator.Current => Current ?? throw new ArgumentNullException(nameof(Current));

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
