using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ArchGame.Extensions;

namespace ArchGame.Components.XnaComponents {
	/// <summary>
	/// A sprite draws a Texture on screen with configurable parameters (position, color, etc)
	/// </summary>
	public class Sprite : IArchLoadable, IArchDrawable {
		readonly string filename;
		Texture2D texture;

		/// <summary>
		/// The position of the sprite.
		/// Default: {0, 0}
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// The source rectangle. Can be null.
		/// Default: null
		/// </summary>
		public Rectangle? SourceRectangle;

		/// <summary>
		/// The color of the tint.
		/// Default: Color.White
		/// </summary>
		public Color Color;

		/// <summary>
		/// The rotation of the sprite.
		/// Default: 0
		/// </summary>
		public float Rotation;

		/// <summary>
		/// The origin coordinates from which to draw the sprite.
		/// Default: Size / 2 if Center; {0, 0} otherwise
		/// </summary>
		public Vector2 Origin;

		/// <summary>
		/// The scale of the sprite.
		/// Default: {1, 1}
		/// </summary>
		public Vector2 Scale;

		/// <summary>
		/// The SpriteEffects of the sprite.
		/// Default: SpriteEffects.None
		/// </summary>
		public SpriteEffects Effects;

		/// <summary>
		/// The layer depth of the sprite.
		/// Default: 0
		/// </summary>
		public int LayerDepth;

		/// <summary>
		/// The ZIndex of the sprite.
		/// Default: 0
		/// </summary>
		public int ZIndex { get; set; }

		/// <summary>
		/// Whether to center the sprite or not (relative to the position)
		/// </summary>
		public bool Center {
			get { return _center; }
			set {
				_center = value;
				CenterSprite();
			}
		}

		bool _center;

		/// <summary>
		/// Initialize a new instance of type Sprite
		/// </summary>
		/// <param name="newFilename">The filename (from the Content project) of the texture</param>
		/// <param name="newZIndex">The ZIndex of the sprite</param>
		/// <param name="newCenter">Whether to center the sprite</param>
		public Sprite(string newFilename, int newZIndex = 0, bool newCenter = false) {
			filename = newFilename;
			Position = new Vector2();
			SourceRectangle = null;
			Color = Color.White;
			Rotation = 0;
			Origin = new Vector2();
			Scale = new Vector2(1);
			Effects = SpriteEffects.None;
			LayerDepth = 0;
			ZIndex = newZIndex;
			Center = newCenter;
		}

		/// <summary>
		/// Initialize a new instance of type Sprite
		/// </summary>
		/// <param name="newFilename">The filename (from the Content project) of the texture</param>
		/// <param name="newPosition">The position of the sprite</param>
		/// <param name="newZIndex">The ZIndex of the sprite</param>
		/// <param name="newCenter">Whether to center the sprite</param>
		public Sprite(string newFilename, Vector2 newPosition, int newZIndex = 0, bool newCenter = false)
			: this (newFilename, newZIndex, newCenter) {
			Position = newPosition;
		}

		/// <summary>
		/// Initialize a new instance of type Sprite as a copy of another Sprite
		/// </summary>
		/// <param name="other">The Sprite to copy drawing parameters from</param>
		public Sprite(Sprite other) {
			filename = other.filename;
			texture = other.texture;
			Position = other.Position;
			SourceRectangle = other.SourceRectangle;
			Color = other.Color;
			Rotation = other.Rotation;
			Origin = other.Origin;
			Scale = other.Scale;
			Effects = other.Effects;
			LayerDepth = other.LayerDepth;
			ZIndex = other.ZIndex;
			Center = other.Center;
		}

		/// <summary>
		/// Loads the Texture2D of the sprite
		/// </summary>
		/// <param name="contentManager">The ContentManager</param>
		public void LoadContent(ContentManager contentManager) {
			texture = contentManager.Load<Texture2D>(filename);
		}

		/// <summary>
		/// Draws the sprite
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch</param>
		public void Draw(SpriteBatch spriteBatch) {
			if (Center) {
				CenterSprite();
			}
			spriteBatch.Draw(texture, Position, SourceRectangle, Color, Rotation, Origin, Scale, Effects, LayerDepth);
		}

		void CenterSprite() {
			Origin = _center ? ((SourceRectangle.HasValue ? SourceRectangle.Value : texture.Bounds).Size() / 2) : new Vector2();
		}

		/// <summary>
		/// Get the local bounds of the Sprite. (relative to the Position)
		/// </summary>
		public Rectangle GetLocalBounds() {
			if (SourceRectangle.HasValue) {
				return new Rectangle((int) (-Origin.X), (int) (-Origin.X), SourceRectangle.Value.Width, SourceRectangle.Value.Height);
			}
			return new Rectangle((int) (-Origin.X), (int) (-Origin.Y), texture.Bounds.Width, texture.Bounds.Height);
		}

		/// <summary>
		/// Get the global bounds of the Sprite.
		/// </summary>
		public Rectangle GetGlobalBounds() {
			Vector2 pos = Position - Origin;
			if (SourceRectangle.HasValue) {
				return new Rectangle((int) pos.X, (int) pos.Y, SourceRectangle.Value.Width, SourceRectangle.Value.Height);
			}
			return new Rectangle((int)pos.X, (int)pos.Y, texture.Bounds.Width, texture.Bounds.Height);
		}
	}
}