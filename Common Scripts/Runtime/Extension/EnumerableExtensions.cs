namespace KnispelCommon.Extension
{
	using System.Collections.Generic;

	public static class EnumerableExtensions
	{
		public static IEnumerable<T> ToEnumerable<T>(params T[] items)
		{
			return items;
		}

		public static IEnumerable<T> ToEnumerable<T>(this T item)
		{
			yield return item;
		}
	}
}
