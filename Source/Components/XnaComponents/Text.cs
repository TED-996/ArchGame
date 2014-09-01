using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.Components.XnaComponents {
	public class Text : IArchLoadable, IArchDrawable {
		readonly string filename;
		SpriteFont spriteFont;
		
		public string TextToDraw {
			get { return text; }
			set {
				text = value;
				sizeDirty = true;
			}
		}

		string text;
		public Vector2 Position;
		public Rectangle? SourceRectangle;
		public Color Color;
		public float Rotation;
		public Vector2 Origin;
		public Vector2 Scale;
		public SpriteEffects Effects;
		public int LayerDepth;

		Vector2 size;
		bool sizeDirty;

		public int ZIndex { get; set; }

		public bool Center {
			get { return _center; }
			set {
				_center = value;
				CenterText();
			}
		}

		bool _center;

		public Text(string newFilename, string newText, int newZIndex = 0, bool newCenter = false) {
			filename = newFilename;
			text = newText;
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

			sizeDirty = true;
		}

		public Text(string newFilename, string newText, Vector2 newPosition, int newZIndex = 0, bool newCenter = false)
			: this (newFilename, newText, newZIndex, newCenter) {
			Position = newPosition;
		}

		public Text(Text other) {
			filename = other.filename;
			spriteFont = other.spriteFont;
			text = other.text;
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
			size = other.size;
			sizeDirty = other.sizeDirty;
		}

		public void LoadContent(ContentManager contentManager) {
			spriteFont = contentManager.Load<SpriteFont>(filename);
		}

		public void Draw(SpriteBatch spriteBatch) {
			if (_center) {
				CenterText();
			}
			spriteBatch.DrawString(spriteFont, text, Position, Color, Rotation, Origin, Scale, Effects, LayerDepth);
		}

		void CenterText() {
			if (sizeDirty) {
				CleanSize();
			}
			Origin = _center ? (size / 2) : new Vector2();
		}

		void CleanSize() {
			size = spriteFont.MeasureString(text);
			sizeDirty = false;
		}
	}
}