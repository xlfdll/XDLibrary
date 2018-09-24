using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Xlfdll.Collections
{
	public static class EnumerableExtensions
	{
		public static void ForEach<T>(this IEnumerable enumeration, Action<T> action)
		{
			foreach (T item in enumeration)
			{
				action(item);
			}
		}

		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration)
			{
				action(item);
			}
		}

		#region ObservableCollection<T>

		public static void AddRange<T>(this ObservableCollection<T> target, IEnumerable<T> source)
		{
			foreach (T item in source)
			{
				target.Add(item);
			}
		}

		public static void AddRange<T1, T2>(this ObservableCollection<T1> target, IEnumerable<T2> source, Func<T2, T1> newItemFunc)
		{
			foreach (T2 item in source)
			{
				target.Add(newItemFunc(item));
			}
		}

		public static void AddRangeUnique<T>(this ObservableCollection<T> target, IEnumerable<T> source)
		{
			foreach (T item in source)
			{
				if (!target.Contains(item))
				{
					target.Add(item);
				}
			}
		}

		public static void AddRangeUnique<T1, T2>(this ObservableCollection<T1> target, IEnumerable<T2> source, Func<T2, T1> newItemFunc)
		{
			foreach (T2 item in source)
			{
				T1 newItem = newItemFunc(item);

				if (!target.Contains(newItem))
				{
					target.Add(newItem);
				}
			}
        }

        public static void RemoveRange<T>(this ObservableCollection<T> target, IEnumerable<T> source)
		{
			Stack<T> selectedItems = new Stack<T>(source);

			while (selectedItems.Count > 0)
			{
				target.Remove(selectedItems.Pop());
			}
		}

		#endregion
	}
}