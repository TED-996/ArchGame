using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ArchGame.Extensions;

namespace ArchGame.Components.XnaComponents {
	public class Sprite : IArchLoadable, IArchDrawable {
		readonly string filename;
		Texture2D texture;

		public Vector2 Position;
		public Rectangle? SourceRectangle;
		public Color Color;
		public float Rotation;
		public Vector2 Origin;
		public Vector2 Scale;
		public SpriteEffects Effects;
		public int LayerDepth;

		public int ZIndex { get; set; }

		public bool Center {
			get { return _center; }
			set {
				_center = value;
				CenterSprite();
			}
		}

		bool _center;

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

		public Sprite(string newFilename, Vector2 newPosition, int newZIndex = 0, bool newCenter = false)
			: this (newFilename, newZIndex, newCenter) {
			Position = newPosition;
		}

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

		public void LoadContent(ContentManager contentManager) {
			texture = contentManager.Load<Texture2D>(filename);
		}

		public void Draw(SpriteBatch spriteBatch) {
			if (Center) {
				CenterSprite();
			}
			spriteBatch.Draw(texture, Position, SourceRectangle, Color, Rotation, Origin, Scale, Effects, LayerDepth);
		}

		void CenterSprite() {
			Origin = _center ? (texture.Bounds.Size() / 2) : new Vector2();
		}
	}
}