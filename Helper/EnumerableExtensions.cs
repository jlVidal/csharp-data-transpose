using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLVidalTranspose.Helper
{
    public static class EnumerableExtensions
    {
        public static T BinarySearchFirst<T>(this IList<T> list, T key, IComparer<T> comparer = null)
        {
            if (list == null)
                throw new NullReferenceException("list");

            if (key == null)
                throw new NullReferenceException("key");

            comparer = comparer ?? Comparer<T>.Default;
            int min = 0;
            int max = list.Count;
            while (min < max)
            {
                int mid = (max + min) / 2;
                T midItem = list[mid];

                int comp = comparer.Compare(midItem, key);
                //int comp = midKey.CompareTo(key);
                if (comp < 0)
                {
                    min = mid + 1;
                }
                else if (comp > 0)
                {
                    max = mid - 1;
                }
                else
                {
                    return midItem;
                }
            }
            if (min == max &&
                //keySelector(list[min]).CompareTo(key) == 0)
                comparer.Compare(list[Math.Min(list.Count - 1, min)], key) == 0)
            {
                return list[min];
            }
            throw new InvalidOperationException("Item not found");
        }

        public static T BinarySearchFirst<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey key, IComparer<TKey> comparer = null)
        {
            if (list == null)
                throw new NullReferenceException("list");

            if (keySelector == null)
                throw new NullReferenceException("keySelector");

            if (key == null)
                throw new NullReferenceException("key");

            comparer = comparer ?? Comparer<TKey>.Default;
            int min = 0;
            int max = list.Count;
            while (min < max)
            {
                int mid = (max + min) / 2;
                T midItem = list[mid];
                TKey midKey = keySelector(midItem);

                int comp = comparer.Compare(midKey, key);
                //int comp = midKey.CompareTo(key);
                if (comp < 0)
                {
                    min = mid + 1;
                }
                else if (comp > 0)
                {
                    max = mid - 1;
                }
                else
                {
                    return midItem;
                }
            }
            if (min == max)
            {
                //keySelector(list[min]).CompareTo(key) == 0)
                var index = Math.Min(list.Count - 1, min);
                if (comparer.Compare(keySelector(list[index]), key) == 0)
                {
                    return list[index];
                }
            }


            throw new InvalidOperationException("Item not found");
        }

        public static IEnumerable<T> BinarySearchMany<T>(this IList<T> list, T key, IComparer<T> comparer = null)
        {
            if (list == null)
                throw new NullReferenceException("list");

            if (list.Count == 0)
                yield break;

            if (key == null)
                throw new NullReferenceException("key");

            comparer = comparer ?? Comparer<T>.Default;
            int min = 0;
            int max = list.Count;
            while (min < max)
            {
                int mid = (max + min) / 2;
                T midItem = list[mid];

                int comp = comparer.Compare(midItem, key);
                if (comp < 0)
                {
                    min = mid + 1;
                }
                else if (comp > 0)
                {
                    max = mid - 1;
                }
                else
                {
                    var cache = new Stack<T>();
                    cache.Push(midItem);

                    var midBelow = mid - 1;
                    while (midBelow >= min)
                    {
                        midItem = list[midBelow];
                        if (0 != comparer.Compare(midItem, key))
                            break;

                        cache.Push(midItem);
                        midBelow = midBelow - 1;
                    }

                    foreach (var order in cache)
                    {
                        yield return order;
                    }
                    cache.Clear();

                    var midAbove = mid + 1;
                    while (midAbove < max)
                    {
                        midItem = list[midAbove];
                        if (0 != comparer.Compare(midItem, key))
                            break;

                        yield return midItem;
                        midAbove = midAbove + 1;
                    }

                    yield break;
                }
            }
            if (min == max)
            {
                var index = Math.Min(list.Count - 1, min);
                if (comparer.Compare(list[index], key) == 0)
                    yield return list[index];
            }
        }

        public static IEnumerable<T> BinarySearchMany<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey key, IComparer<TKey> comparer = null)
        {
            if (list == null)
                throw new NullReferenceException("list");

            if (list.Count == 0)
                yield break;

            if (keySelector == null)
                throw new NullReferenceException("keySelector");

            if (key == null)
                throw new NullReferenceException("key");

            comparer = comparer ?? Comparer<TKey>.Default;
            int min = 0;
            int max = list.Count;
            while (min < max)
            {
                int mid = (max + min) / 2;
                T midItem = list[mid];
                TKey midKey = keySelector(midItem);

                int comp = comparer.Compare(midKey, key);
                if (comp < 0)
                {
                    min = mid + 1;
                }
                else if (comp > 0)
                {
                    max = mid - 1;
                }
                else
                {
                    var cache = new Stack<T>();
                    cache.Push(midItem);

                    var midBelow = mid - 1;
                    while (midBelow >= min)
                    {
                        midItem = list[midBelow];
                        midKey = keySelector(midItem);
                        if (0 != comparer.Compare(midKey, key))
                        {
                            break;
                        }

                        cache.Push(midItem);
                        midBelow = midBelow - 1;
                    }

                    foreach (var order in cache)
                    {
                        yield return order;
                    }
                    cache.Clear();

                    var midAbove = mid + 1;
                    while (midAbove < max)
                    {
                        midItem = list[midAbove];
                        midKey = keySelector(midItem);
                        if (0 != comparer.Compare(midKey, key))
                        {
                            break;
                        }

                        yield return midItem;
                        midAbove = midAbove + 1;
                    }

                    yield break;
                }
            }

            if (min == max)
            {
                var index = Math.Min(list.Count - 1, min);
                if (comparer.Compare(keySelector(list[index]), key) == 0)
                    yield return list[index];
            }

        }
    }
}
