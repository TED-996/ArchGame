using System;
using System.Text;
using System.Linq;

namespace ArchGame.Extensions {
	public static class MiscExtensions {
		public static TimeSpan Multiply(this TimeSpan timeSpan, double factor) {
			return new TimeSpan((long)(timeSpan.Ticks * factor));
		}

		public static string Multiply(this string thisString, int times) {
			const int efficiencyTreshold = 2;
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

		public static string Format(this string value, params object[] args) {
			return string.Format(value, args);
		}
	}
}