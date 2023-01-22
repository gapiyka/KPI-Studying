using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace CustomList
{
    public delegate void Notify();
    public class CustomList<T> : ICollection<T>
    {
        private const int defaultCapacity = 2;
        private T[] values;
        private int count = 0;
        public event EventHandler<int> Handler;
        public CustomList()
        {
            values = new T[defaultCapacity];
        }

        public CustomList(IEnumerable ie)
        {
            values = new T[defaultCapacity];
            foreach (T i in ie)
            {
                Add(i);
            }
        }

        public CustomList(int initCapacity)
        {
            if (initCapacity < 1) initCapacity = defaultCapacity;
            values = new T[initCapacity];
        }

        public int Count { get { return count; } }
        public bool IsEmpty { get { return count == 0; } }
        public bool IsReadOnly { get { return values.IsReadOnly; } }

        public T this[int index]
        {
            get
            {
                return Get(index);
            }
            set
            {
                Set(value, index);
            }
        }
        public T Get(int index)
        {
            index = CheckIfIndexOutOfRange(index);
            return values[index];
        }

        public void Set(T newElement, int index)
        {
            index = CheckIfIndexOutOfRange(index);
            values[index] = newElement;
        }

        public void Insert(int index, T newElement)
        {
            CheckIfEnoughSpace();
            index = CheckIfIndexOutOfRange(index);

            for (int i = count; i > index; i--)
            {
                values[i] = values[i - 1];
            }

            values[index] = newElement;
            NotifyCollectionChanged (++count);
        }

        public void Remove(int index)
        {
            CheckIfIndexOutOfRange(index);
            T[] newArr = new T[--count];
            for (int i = 0; i < count; i++)
            {
                newArr[i] = (i < index) ? values[i] : values[i + 1];
            }
            values = newArr;
            NotifyCollectionChanged (count);
        }

        public bool Remove(T item)
        {
            bool res;
            int itemIndex = Find(item);
            if (itemIndex == -1)
            {
                res = false;
                NotifyCollectionChanged (count);
            } else
            {
                Remove(itemIndex);
                res = true;
            }
            return res;
        }

        public void Add(T newElement)
        {
            CheckIfEnoughSpace();
            values[count++] = newElement;
            NotifyCollectionChanged (count);
        }

        public bool Contains(T value)
        {
            for (int i = 0; i < count; i++)
            {
                T currentValue = values[i];
                if (currentValue.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public int Find(T value)
        {
            for (int i = 0; i < count; i++)
            {
                T currentValue = values[i];
                if (currentValue.Equals(value))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Clear()
        {
            count = 0;
            values = new T[count];
            NotifyCollectionChanged (count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T Titem in values)
            {
                yield return Titem;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < Count; i++)
            {
                array.SetValue(values[i], arrayIndex++);
            }
        }

        private void Resize()
        {
            int newSize = values.Length * 2;
            T[] newArr = new T[newSize];
            for (int i = 0; i < count; i++)
            {
                newArr[i] = values[i];
            }
            values = newArr;
        }

        private void CheckIfEnoughSpace()
        {
            if (count == values.Length)
            {
                Resize();
            }
        }

        private int CheckIfIndexOutOfRange(int index)
        {
            return (index > count) ? index - count : index;
        }

        protected virtual void NotifyCollectionChanged (int count)
        {
            Handler?.Invoke(this, count);
        }
    }
}

