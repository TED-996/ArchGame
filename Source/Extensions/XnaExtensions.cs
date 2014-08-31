using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.Extensions {
	public static class XnaExtensions {
		public static Vector3 GetVector3(this Vector2 vector) {
			return new Vector3(-vector.Y, 0, vector.X);
		}

		public static Vector2 GetVector2(this Vector3 vector) {
			return new Vector2(-vector.Z, vector.X);
		}

		public static Point ToPoint(this Vector2 vector) {
			return new Point((int) vector.X, (int) vector.Y);
		}

		public static Vector2 ToVector2(this Point point) {
			return new Vector2(point.X, point.Y);
		}

		public static Vector2 GetFlipped(this Vector2 vector) {
			return new Vector2(vector.Y, vector.X);
		}

		public static Vector2 TopLeft(this Rectangle rectangle) {
			return new Vector2(rectangle.X, rectangle.Y);
		}

		public static Vector2 BottomRight(this Rectangle rectangle) {
			return new Vector2(rectangle.Right, rectangle.Bottom);
		}

		public static Vector2 Size(this Rectangle rectangle) {
			return new Vector2(rectangle.Width, rectangle.Height);
		}

		public static Rectangle GetRelative(this Rectangle thisRect, Rectangle otherRect) {
			return new Rectangle(thisRect.X + otherRect.X, thisRect.Y + otherRect.Y, otherRect.Width, otherRect.Height);
		}

		public static Vector2 GetRelative(this Rectangle thisRect, Vector2 position) {
			return new Vector2(thisRect.X + position.X, thisRect.Y + position.Y);
		}

		public static bool IsStrictlyPositive(this Rectangle thisRect) {
			return thisRect.Height > 0 && thisRect.Width > 0;
		}

		public static Rectangle RectangleFromPoints(Point p1, Point p2) {
			return new Rectangle(p1.X.AtMost(p2.X), p1.Y.AtMost(p2.Y), (p1.X - p2.X).Abs(), (p1.Y - p2.Y).Abs());
		}

		public static Rectangle RectangleFromCoords(int x1, int y1, int x2, int y2) {
			return RectangleFromPoints(new Point(x1, y1), new Point(x2, y2));
		}

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