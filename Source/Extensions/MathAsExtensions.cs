﻿using System;

namespace ArchGame.Extensions {
	/// <summary>
	/// Static class that exposes several math functions as extension methods.
	/// </summary>
	public static class MathAsExtensions {
		/// <summary>
		/// Returns the absolute value of an int.
		/// </summary>
		public static int Abs(this int value) {
			return Math.Abs(value);
		}

		/// <summary>
		/// Returns the absolute value of a float.
		/// </summary>
		public static float Abs(this float value) {
			return Math.Abs(value);
		}

		/// <summary>
		/// Returns the absolute value of a double.
		/// </summary>
		public static double Abs(this double value) {
			return Math.Abs(value);
		}

		/// <summary>
		/// Returns the absolute value of a long.
		/// </summary>
		public static long Abs(this long value) {
			return Math.Abs(value);
		}

		/// <summary>
		/// Returns the absolute value of a decimal.
		/// </summary>
		public static decimal Abs(this decimal value) {
			return Math.Abs(value);
		}

		/// <summary>
		/// Returns the absolute value of a sbyte.
		/// </summary>
		public static sbyte Abs(this sbyte value) {
			return Math.Abs(value);
		}

		/// <summary>
		/// Returns the absolute value of a short.
		/// </summary>
		public static short Abs(this short value) {
			return Math.Abs(value);
		}

		/// <summary>
		/// Constrains thisValue to be at least minValue.
		/// Returns thisValue if it's greater than minValue or minValue otherwise.
		/// Equvalent to Math.Max(thisValue, minValue) or { thisValue &gt; minValue ? thisValue : minValue }
		/// </summary>
		public static T AtLeast<T>(this T thisValue, T minValue) where T : IComparable<T> {
			return thisValue.CompareTo(minValue) > 0 ? thisValue : minValue;
		}

		/// <summary>
		/// Constrains thisValue to be at most maxValue.
		/// Returns thisValue if it's less than maxValue or maxValue otherwise.
		/// Equvalent to Math.Min(thisValue, maxValue) or { thisValue &lt; maxValue ? thisValue : maxValue }
		/// </summary>
		public static T AtMost<T>(this T thisValue, T maxValue) where T : IComparable<T> {
			return thisValue.CompareTo(maxValue) < 0 ? thisValue : maxValue;
		}

		/// <summary>
		/// Clamps val between min and max.
		/// Returns min if val is less than min, max if val is greater than max or val otherwise.
		/// Equivalent to { val &lt; min ? min : (val &gt; max ? max : val) }
		/// </summary>
		public static T Clamp<T>(this T val, T min, T max) where T :IComparable<T> {
			if (val.CompareTo(min) < 0) {
				return min;
			}
			if (val.CompareTo(max) > 0) {
				return max;
			}
			return val;
		}
		
		/// <summary>
		/// Rounds a double value to the nearest integer.
		/// </summary>
		public static int RoundToInt(this double thisValue) {
			return (int) Math.Round(thisValue);
		}

		/// <summary>
		/// Rounds a float value to the nearest integer.
		/// </summary>
		public static int RoundToInt(this float thisValue) {
			return (int) Math.Round(thisValue);
		}
	}
}