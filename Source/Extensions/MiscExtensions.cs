using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ArchGame.Extensions {
	public static class MiscExtensions {
		/// <summary>
		/// Multiplies a TimeSpan a certain number of times.
		/// </summary>
		/// <param name="timeSpan">The TimeSpan to multiply</param>
		/// <param name="factor">The factor to multiply the TimeSpan with</param>
		/// <returns></returns>
		public static TimeSpan Multiply(this TimeSpan timeSpan, double factor) {
			return new TimeSpan((long)(timeSpan.Ticks * factor));
		}

		/// <summary>
		/// Returns a string multiplied a certain number of times.
		/// "123".Multiply(3) returns "123123123"
		/// </summary>
		/// <param name="thisString">The string to multiply</param>
		/// <param name="times">How many times to multiply the string.</param>
		public static string Multiply(this string thisString, int times) {
			const int efficiencyTreshold = 3;
			if (times <= efficiencyTreshold) {
				return MultiplyShortPath(thisString, times);
			}

			StringBuilder result = new StringBuilder(thisString, thisString.Length * times);
			while (times > 1) {
				result.Append(thisString);
				times--;
			}
			return result.ToString();
		}

		static string MultiplyShortPath(this string thisString, int times) {
			string result = thisString;
			while (times > 1) {
				result += thisString;
				times--;
			}
			return result;
		}

		public static bool IsNull<T>(this T value) where T : class {
			return value == null;
		}

		public static bool IsNotNull<T>(this T value) where T : class {
			return value != null;
		}

		/// <summary>
		/// Throws ArgumentNullException if value is null. Returns the value for method chaining.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value to check against null</param>
		/// <param name="argumentName">The name of the argument to be used as parameter to the ArgumentNullException</param>
		/// <returns>The value, to be used for method chaining.</returns>
		public static T ThrowIfNull<T>(this T value, string argumentName = "unknown") where T : class {
			if (value == null) {
				throw new ArgumentNullException(argumentName);
			}
			return value;
		}

		/// <summary>
		/// Returns true if the value is in the list.
		/// Potentially expensive convenience method.
		/// </summary>
		/// <param name="value">The value to check</param>
		/// <param name="list">The list of values to check against</param>
		/// <returns>True if the element exists, false oftherwise</returns>
		public static bool IsOneOf<T>(this T value, params T[] list) {
			return list.Contains(value);
		}

		/// <summary>
		/// String.Format as extension method
		/// </summary>
		public static string Format(this string value, params object[] args) {
			return string.Format(value, args);
		}

		/// <summary>
		/// Sorts a list using LINQ's stable sort.
		/// Potentially expensive for large list sizes.
		/// </summary>
		/// <typeparam name="T">The list type</typeparam>
		/// <param name="list">The list to sort</param>
		/// <param name="comparer">A Comparison&lt;T&gt; to use when sorting</param>
		public static void StableSort<T>(this List<T> list, Comparison<T> comparer) {
			List<T> orderedList = list.OrderBy(k => k, new ComparisonToIComparer<T>(comparer)).ToList();
			list.Clear();
			list.AddRange(orderedList);
		}

		class ComparisonToIComparer<T> : IComparer<T> {
			readonly Comparison<T> comparison;

			public ComparisonToIComparer(Comparison<T> newComparison) {
				comparison = newComparison;
			}  

			public int Compare(T x, T y) {
				return comparison(x, y);
			}
		} 
	}
}