using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.Extensions {
	public static class SpriteBatchExtensions {
		public static void DrawTiled(this SpriteBatch spriteBatch, Texture2D texture, Rectangle position,
			Rectangle? sourceRectangleN, Color color) {
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

		public static void DrawTiled(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position1, Vector2 position2,
									 Rectangle? sourceRectangle, Color color) {
			Rectangle positionRect = new Rectangle((int)position1.X, (int)position1.Y,
				(int)(position2 - position1).X.Abs(), (int)(position2 - position1).Y.Abs());
			spriteBatch.DrawTiled(texture, positionRect, sourceRectangle, color);
		}

		public static void DrawClipped(this SpriteBatch spriteBatch, Texture2D texture, Rectangle position,
									   Rectangle? sourceRectangleN, Color color, int corner = 1) {
			if (!sourceRectangleN.HasValue) {
				spriteBatch.Draw(texture, position.TopLeft(), Color.White);
				return;
			}
			Rectangle sourceRectangle = sourceRectangleN.Value;
			if (sourceRectangle.Height <= position.Height && sourceRectangle.Width <= position.Width) {
				spriteBatch.Draw(texture, position.TopLeft(), Color.White);
			}
			Rectangle finalSourceRectangle;
			if (corner == 1) {
				finalSourceRectangle = new Rectangle(sourceRectangle.X, sourceRectangle.Y,
					sourceRectangle.Width.AtMost(position.Width), sourceRectangle.Height.AtMost(position.Height));
			}
			else if (corner == 2) {
				finalSourceRectangle =
					XnaExtensions.RectangleFromCoords((sourceRectangle.Right - position.Width).AtLeast(sourceRectangle.X),
													   sourceRectangle.Y, sourceRectangle.Right,
													   sourceRectangle.Bottom.AtMost(sourceRectangle.Y + position.Height));
			}
			else if (corner == 3) {
				finalSourceRectangle =
					XnaExtensions.RectangleFromCoords((sourceRectangle.Right - position.Width).AtLeast(sourceRectangle.X),
													   (sourceRectangle.Bottom - position.Height).AtLeast(sourceRectangle.Y),
													   sourceRectangle.Right, sourceRectangle.Bottom);
			}
			else if (corner == 4) {
				finalSourceRectangle = XnaExtensions.RectangleFromCoords(sourceRectangle.X,
																		  (sourceRectangle.Bottom - position.Height).AtLeast(
																			  sourceRectangle.Y),
																		  sourceRectangle.Right.AtMost(sourceRectangle.X +
																									position.Height),
																		  sourceRectangle.Bottom);
			}
			else {
				throw new ApplicationException("Bad corner in DrawClipped.");
			}
			spriteBatch.Draw(texture, position.TopLeft(), finalSourceRectangle, Color.White);
		}

		public static void DrawSmart(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle,
									 Rectangle? sourceRectangleN, Color color, int borderPercentage) {
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
			spriteBatch.DrawTiled(texture, XnaExtensions.RectangleFromCoords(x1D, y0D, x2D, y1D),
				XnaExtensions.RectangleFromCoords(x1S, y0S, x2S, y1S), Color.White);
			spriteBatch.Draw(texture, XnaExtensions.RectangleFromCoords(x2D, y0D, x3D, y1D),
				XnaExtensions.RectangleFromCoords(x2S, y0S, x3S, y1S), Color.White);
			spriteBatch.DrawTiled(texture, XnaExtensions.RectangleFromCoords(x0D, y1D, x1D, y2D),
				XnaExtensions.RectangleFromCoords(x0S, y1S, x1S, y2S), Color.White);
			spriteBatch.DrawTiled(texture, XnaExtensions.RectangleFromCoords(x1D, y1D, x2D, y2D),
				XnaExtensions.RectangleFromCoords(x1S, y1S, x2S, y2S), Color.White);
			spriteBatch.DrawTiled(texture, XnaExtensions.RectangleFromCoords(x2D, y1D, x3D, y2D),
				XnaExtensions.RectangleFromCoords(x2S, y1S, x3S, y2S), Color.White);
			spriteBatch.Draw(texture, XnaExtensions.RectangleFromCoords(x0D, y2D, x1D, y3D),
				XnaExtensions.RectangleFromCoords(x0S, y2S, x1S, y3S), Color.White);
			spriteBatch.DrawTiled(texture, XnaExtensions.RectangleFromCoords(x1D, y2D, x2D, y3D),
				XnaExtensions.RectangleFromCoords(x1S, y2S, x2S, y3S), Color.White);
			spriteBatch.Draw(texture, XnaExtensions.RectangleFromCoords(x2D, y2D, x3D, y3D),
				XnaExtensions.RectangleFromCoords(x2S, y2S, x3S, y3S), Color.White);
		}

	}
}