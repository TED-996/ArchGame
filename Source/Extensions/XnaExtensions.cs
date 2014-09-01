using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.Extensions {
	public static class XnaExtensions {
		/// <summary>
		/// Get a Vector3 from a Vector2. Consult the source code to see if this is an intended converison.
		/// </summary>
		public static Vector3 GetVector3(this Vector2 vector) {
			return new Vector3(-vector.Y, 0, vector.X);
		}

		/// <summary>
		/// Get a Vector2 from a Vector3. Consult the source code to see if this is an intended converison.
		/// </summary>
		public static Vector2 GetVector2(this Vector3 vector) {
			return new Vector2(-vector.Z, vector.X);
		}

		/// <summary>
		/// Get a Point from a Vector2.
		/// </summary>
		public static Point ToPoint(this Vector2 vector) {
			return new Point((int) vector.X, (int) vector.Y);
		}

		/// <summary>
		/// Get a Vector2 from a Point.
		/// </summary>
		public static Vector2 ToVector2(this Point point) {
			return new Vector2(point.X, point.Y);
		}

		/// <summary>
		/// Flip a Vector2, as in, swap its components.
		/// </summary>
		public static Vector2 GetFlipped(this Vector2 vector) {
			return new Vector2(vector.Y, vector.X);
		}

		/// <summary>
		/// Get the top-left corner of a rectangle.
		/// </summary>
		public static Vector2 TopLeft(this Rectangle rectangle) {
			return new Vector2(rectangle.X, rectangle.Y);
		}

		/// <summary>
		/// Get the bottom-right corner of a rectangle.
		/// </summary>
		public static Vector2 BottomRight(this Rectangle rectangle) {
			return new Vector2(rectangle.Right, rectangle.Bottom);
		}

		/// <summary>
		/// Get the size of a rectangle as a Vector2.
		/// </summary>
		public static Vector2 Size(this Rectangle rectangle) {
			return new Vector2(rectangle.Width, rectangle.Height);
		}

		/// <summary>
		/// Get the first rectangle relative to the second. Consult the source code to see if this is intended behaviour.
		/// </summary>
		public static Rectangle GetRelative(this Rectangle thisRect, Rectangle otherRect) {
			return new Rectangle(thisRect.X + otherRect.X, thisRect.Y + otherRect.Y, otherRect.Width, otherRect.Height);
		}

		/// <summary>
		/// Get the Vector2 relative to the rectangle. Consult the source code to see if this is intended behaviour.
		/// </summary>
		public static Vector2 GetRelative(this Rectangle thisRect, Vector2 position) {
			return new Vector2(thisRect.X + position.X, thisRect.Y + position.Y);
		}

		/// <summary>
		/// Returns true if both a rectangle's width and height are strictly positive.
		/// </summary>
		public static bool IsStrictlyPositive(this Rectangle thisRect) {
			return thisRect.Height > 0 && thisRect.Width > 0;
		}

		/// <summary>
		/// Constructs a rectangle from two points.
		/// </summary>
		public static Rectangle RectangleFromPoints(Point p1, Point p2) {
			return new Rectangle(p1.X.AtMost(p2.X), p1.Y.AtMost(p2.Y), (p1.X - p2.X).Abs(), (p1.Y - p2.Y).Abs());
		}

		/// <summary>
		/// Construct a rectangle from two coordinates.
		/// </summary>
		public static Rectangle RectangleFromCoords(int x1, int y1, int x2, int y2) {
			return RectangleFromPoints(new Point(x1, y1), new Point(x2, y2));
		}

		/// <summary>
		/// Get the center of a Model.
		/// </summary>
		public static Vector3 GetCenter(this Model model) {
			Vector3 modelCenter = new Vector3(0);

			// Look up the absolute bone transforms for this model.
			Matrix[] boneTransforms = new Matrix[model.Bones.Count];

			model.CopyAbsoluteBoneTransformsTo(boneTransforms);

			// Compute an (approximate) model center position by
			// averaging the center of each mesh bounding sphere.

			foreach (ModelMesh mesh in model.Meshes) {
				modelCenter += Vector3.Transform(mesh.BoundingSphere.Center, boneTransforms[mesh.ParentBone.Index]);
			}

			modelCenter /= model.Meshes.Count;

			return modelCenter;
		}
	}
}