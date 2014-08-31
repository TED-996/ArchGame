using System;

namespace ArchGame.Extensions {
	public static class MathAsExtensions {
		public static int Abs(this int value) {
			return Math.Abs(value);
		}

		public static float Abs(this float value) {
			return Math.Abs(value);
		}

		public static double Abs(this double value) {
			return Math.Abs(value);
		}

		public static long Abs(this long value) {
			return Math.Abs(value);
		}

		public static decimal Abs(this decimal value) {
			return Math.Abs(value);
		}

		public static sbyte Abs(this sbyte value) {
			return Math.Abs(value);
		}

		public static short Abs(this short value) {
			return Math.Abs(value);
		}

		public static T AtLeast<T>(this T thisValue, T minValue) where T : IComparable<T> {
			return thisValue.CompareTo(minValue) > 0 ? thisValue : minValue;
		}

		public static T AtMost<T>(this T thisValue, T maxValue) where T : IComparable<T> {
			return thisValue.CompareTo(maxValue) < 0 ? thisValue : maxValue;
		}

		public static T Clamp<T>(this T val, T min, T max) where T :IComparable<T> {
			if (val.CompareTo(min) < 0) {
				return min;
			}
			if (val.CompareTo(max) > 0) {
				return max;
			}
			return val;
		}

		public static int RoundToInt(this double thisValue) {
			return (int) Math.Round(thisValue);
		}

		public static int RoundToInt(this float thisValue) {
			return (int) Math.Round(thisValue);
		}
	}
}