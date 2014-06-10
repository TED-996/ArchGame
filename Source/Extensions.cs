using FarseerPhysics;
using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.Window;

namespace SFMLGame {
	public static class Extensions {
		public static float ToSimUnits(this int displayUnits) {
			return ConvertUnits.ToSimUnits(displayUnits);
		}

		public static float ToSimUnits(this float displayUnits) {
			return ConvertUnits.ToSimUnits(displayUnits);
		}

		public static Vector2 ToSimUnits(this Vector2 displayUnits) {
			return ConvertUnits.ToSimUnits(displayUnits);
		}

		public static float ToSimUnits(this double displayUnits) {
			return ConvertUnits.ToSimUnits(displayUnits);
		}

		public static Vector3 ToSimUnits(this Vector3 displayUnits) {
			return ConvertUnits.ToSimUnits(displayUnits);
		}

		public static float ToDisplayUnits(this int simUnits) {
			return ConvertUnits.ToDisplayUnits(simUnits);
		}

		public static float ToDisplayUnits(this float simUnits) {
			return ConvertUnits.ToDisplayUnits(simUnits);
		}

		public static Vector2 ToDisplayUnits(this Vector2 simUnits) {
			return ConvertUnits.ToDisplayUnits(simUnits);
		}

		public static float ToDisplayUnits(this double simUnits) {
			return ConvertUnits.ToDisplayUnits((float) simUnits);
		}

		public static Vector3 ToDisplayUnits(this Vector3 simUnits) {
			return ConvertUnits.ToDisplayUnits(simUnits);
		}

		public static Vertex ToVertex(this Vector2 vector) {
			vector = vector.ToDisplayUnits();
			return new Vertex(new Vector2f(vector.X, vector.Y));
		}

	}
}