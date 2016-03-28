using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VilRan.Collections
{
    public class WeightedList<T> : IEnumerable<T>
    {
        private static Random DefaultRandom = new Random();

        List<WeightedListNode<T>> items = new List<WeightedListNode<T>>();
        double totalWeight;

        public T this[int i] { get { return items[i].Item; } }
        public double TotalWeight { get { return totalWeight; } }
        public int Count { get { return items.Count; } }

        public WeightedList()
        {
        }

        public void Add(T item, double weight)
        {
            items.Add(new WeightedListNode<T>(item, weight));
            totalWeight += weight;
        }

        public void AddRange(IEnumerable<WeightedListNode<T>> items)
        {
            foreach (WeightedListNode<T> node in items)
            {
                Add(node.Item, node.Weight);
            }
        }

        public void Remove(T item)
        {
            int i = items.FindIndex(e => e.Item.Equals(item));
            totalWeight -= items[i].Weight;
            items.RemoveAt(i);
        }

        public void RemoveAt(int index)
        {
            totalWeight -= items[index].Weight;
            items.RemoveAt(index);
        }

        public void Clear()
        {
            items.Clear();
            totalWeight = 0;
        }

        public void ChangeWeight(T item, double newWeight)
        {
            int i = items.FindIndex(e => e.Item.Equals(item));
            totalWeight -= items[i].Weight;
            totalWeight += newWeight;
            items[i].Weight = newWeight;
        }

        public double GetWeightAt(int i)
        {
            return items[i].Weight;
        }

        /// <summary>
        /// Returns default(T) if the list is empty.
        /// </summary>
        /// <returns></returns>
        public T SelectRandom(Random random = null)
        {
            if (random == null)
                random = DefaultRandom;

            double rn = random.NextDouble() * totalWeight;
            for (int i = 0; i < items.Count; i++)
            {
                if (rn <= items[i].Weight)
                {
                    return items[i].Item;
                }
                rn -= items[i].Weight;
            }
            return default(T);
        }

        /// <summary>
        /// Returns -1 if the list is empty.
        /// </summary>
        /// <returns></returns>
        public int SelectRandomIndex(Random random = null)
        {
            if (random == null)
                random = DefaultRandom;

            int i = -1;
            double rn = random.NextDouble() * totalWeight;
            while (i < items.Count)
            {
                i++;
                if (rn <= items[i].Weight)
                {
                    break;
                }
                rn -= items[i].Weight;
            }
            return i;
        }

        /// <summary>
        /// Returns default(T) if the list is empty.
        /// </summary>
        /// <returns></returns>
        public T PopRandom(Random random = null)
        {
            if (random == null)
                random = DefaultRandom;

            int i = SelectRandomIndex(random);
            if (i >= 0)
            {
                T item = this[i];
                RemoveAt(i);
                return item;
            }
            return default(T);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (WeightedListNode<T> item in items)
            {
                yield return item.Item;
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (WeightedListNode<T> item in items)
            {
                yield return item.Item;
            }
        }
    }

    public class WeightedListNode<T>
    {
        public T Item;
        public double Weight;

        public WeightedListNode(T item, double weight)
        {
            Item = item;
            Weight = weight;
        }
    }
}
