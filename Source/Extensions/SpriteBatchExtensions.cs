using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.Extensions {
	public static class SpriteBatchExtensions {
		/// <summary>
		/// Draws a Texture2D with tiling.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to draw with</param>
		/// <param name="texture">The texture to draw</param>
		/// <param name="position">The rectangle on which to draw</param>
		/// <param name="color">The color of the tint to use. If no tint is needed, use Color.White</param>
		/// <param name="sourceRectangleN">The source rectangle to draw. May be null.</param>
		public static void DrawTiled(this SpriteBatch spriteBatch, Texture2D texture, Rectangle position,
			Color color, Rectangle? sourceRectangleN = null) {
			if (!sourceRectangleN.HasValue) {
				sourceRectangleN = texture.Bounds;
			}
			Rectangle sourceRectangle = sourceRectangleN.Value;
			if (!position.IsStrictlyPositive() || !sourceRectangle.IsStrictlyPositive()) {
				return;
			}
			Vector2 topLeft = new Vector2(position.X, position.Y);
			for (int drawingX = 0; drawingX <= position.Width; drawingX += sourceRectangle.Width) {
				for (int drawingY = 0; drawingY <= position.Height; drawingY += sourceRectangle.Height) {
					Rectangle finalSourceRect = sourceRectangle;
					finalSourceRect.Width = finalSourceRect.Width.AtMost(position.Width - drawingX);
					finalSourceRect.Height = finalSourceRect.Height.AtMost(position.Height - drawingY);
					spriteBatch.Draw(texture, new Vector2(drawingX, drawingY) + topLeft, finalSourceRect, color);
				}
			}
		}

		/// <summary>
		/// Draws a Texture2D with tiling.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to draw with</param>
		/// <param name="texture">The texture to draw</param>
		/// <param name="position1">The first corner of the rectangle on which to draw</param>
		/// <param name="position2">The second corner of the rectangle on which to draw</param>
		/// <param name="color">The color of the tint to use. If no tint is needed, use Color.White</param>
		/// <param name="sourceRectangle">The source rectangle to draw. May be null.</param>
		public static void DrawTiled(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position1, Vector2 position2,
		                             Color color, Rectangle? sourceRectangle) {
			Rectangle positionRect = new Rectangle((int) position1.X, (int) position1.Y, (int) (position2 - position1).X.Abs(),
			                                       (int) (position2 - position1).Y.Abs());
			spriteBatch.DrawTiled(texture, positionRect, color, sourceRectangle);
		}

		/// <summary>
		/// Draws a Texture2D using a special algorithm, that fills the destinationRectangle with the texture.
		/// The borders of the destinationRectangle will be filled with the borders and corners of the source texture.
		/// The center of the destination rectangle will be filled with the center of the source texture.
		/// Absolutely no stretching will occur, only tiling.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to draw with.</param>
		/// <param name="texture">The texture to use</param>
		/// <param name="destinationRectangle">The rectangle to draw on</param>
		/// <param name="sourceRectangleN">The source rectangle to draw. May be null.</param>
		/// <param name="color">The color of the tint to use. Use Color.White for no tint.</param>
		/// <param name="borderPercentage">The percentage of the source rectangle (or border bounds) to consider the border.</param>
		public static void DrawSmart(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle,
									 Color color, Rectangle? sourceRectangleN = null, int borderPercentage = 10) {
			if (!sourceRectangleN.HasValue) {
				sourceRectangleN = texture.Bounds;
			}
			Rectangle sourceRectangle = sourceRectangleN.Value;
			int borderPixelsX = borderPercentage * sourceRectangle.Width / 100;
			int borderPixelsY = borderPercentage * sourceRectangle.Height / 100;

			int x0D = destinationRectangle.X;
			int x1D = borderPixelsX + destinationRectangle.X;
			int x2D = destinationRectangle.Right - borderPixelsX;
			int x3D = destinationRectangle.Right;
			if (x2D < x1D) {
				x1D = (destinationRectangle.Width / 2) + destinationRectangle.X;
				x2D = x1D;
			}
			int y0D = destinationRectangle.Y;
			int y1D = borderPixelsY + destinationRectangle.Y;
			int y2D = destinationRectangle.Bottom - borderPixelsY;
			int y3D = destinationRectangle.Bottom;
			if (y2D < y1D) {
				y1D = (destinationRectangle.Height / 2) + destinationRectangle.Y;
				y2D = y1D;
			}

			int x0S = sourceRectangle.X;
			int x1S = borderPixelsX + sourceRectangle.X;
			int x2S = sourceRectangle.Right - borderPixelsX;
			int x3S = sourceRectangle.Right;
			if (x2S < x1S) {
				x1S = (sourceRectangle.Width / 2) + sourceRectangle.X;
				x2S = x1S;
			}
			int y0S = sourceRectangle.Y;
			int y1S = borderPixelsY + sourceRectangle.Y;
			int y2S = sourceRectangle.Bottom - borderPixelsY;
			int y3S = sourceRectangle.Bottom;
			if (y2S < y1S) {
				y1S = (sourceRectangle.Height / 2) + sourceRectangle.Y;
				y2S = y1S;
			}

			spriteBatch.Draw(texture, XnaExtensions.RectangleFromCoords(x0D, y0D, x1D, y1D),
							 XnaExtensions.RectangleFromCoords(x0S, y0S, x1S, y1S), Color.White);
			spriteBatch.DrawTiled(texture, XnaExtensions.RectangleFromCoords(x1D, y0D, x2D, y1D), Color.White,
				XnaExtensions.RectangleFromCoords(x1S, y0S, x2S, y1S));
			spriteBatch.Draw(texture, XnaExtensions.RectangleFromCoords(x2D, y0D, x3D, y1D),
				XnaExtensions.RectangleFromCoords(x2S, y0S, x3S, y1S), Color.White);
			spriteBatch.DrawTiled(texture, XnaExtensions.RectangleFromCoords(x0D, y1D, x1D, y2D), Color.White,
				XnaExtensions.RectangleFromCoords(x0S, y1S, x1S, y2S));
			spriteBatch.DrawTiled(texture, XnaExtensions.RectangleFromCoords(x1D, y1D, x2D, y2D), Color.White,
				XnaExtensions.RectangleFromCoords(x1S, y1S, x2S, y2S));
			spriteBatch.DrawTiled(texture, XnaExtensions.RectangleFromCoords(x2D, y1D, x3D, y2D), Color.White,
				XnaExtensions.RectangleFromCoords(x2S, y1S, x3S, y2S));
			spriteBatch.Draw(texture, XnaExtensions.RectangleFromCoords(x0D, y2D, x1D, y3D),
				XnaExtensions.RectangleFromCoords(x0S, y2S, x1S, y3S), Color.White);
			spriteBatch.DrawTiled(texture, XnaExtensions.RectangleFromCoords(x1D, y2D, x2D, y3D), Color.White,
				XnaExtensions.RectangleFromCoords(x1S, y2S, x2S, y3S));
			spriteBatch.Draw(texture, XnaExtensions.RectangleFromCoords(x2D, y2D, x3D, y3D),
				XnaExtensions.RectangleFromCoords(x2S, y2S, x3S, y3S), Color.White);
		}

	}
}