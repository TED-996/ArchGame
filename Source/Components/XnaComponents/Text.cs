using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.Components.XnaComponents {
	/// <summary>
	/// A Text draws a string on screen with configurable parameters (font, position, color)
	/// </summary>
	public class Text : IArchLoadable, IArchDrawable {
		readonly string filename;
		SpriteFont spriteFont;
		
		/// <summary>
		/// The text to draw.
		/// </summary>
		public string TextToDraw {
			get { return _text; }
			set {
				_text = value;
				sizeDirty = true;
			}
		}
		string _text;

		/// <summary>
		/// The position of the text.
		/// Default: {0, 0}
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// The Color of the text.
		/// Default: Color.Black
		/// </summary>
		public Color Color;

		/// <summary>
		/// The rotation of the text.
		/// Default: 0
		/// </summary>
		public float Rotation;

		/// <summary>
		/// The origin of the text.
		/// Default: Size / 2 if centered; {0, 0} otherwise.
		/// </summary>
		public Vector2 Origin;

		/// <summary>
		/// The scale of the text.
		/// Default: {1, 1}
		/// </summary>
		public Vector2 Scale;

		/// <summary>
		/// The SpriteEffects of the text.
		/// Default: SpriteEffects.None
		/// </summary>
		public SpriteEffects Effects;

		/// <summary>
		/// The layer depth of the text.
		/// Default: 0
		/// </summary>
		public int LayerDepth;

		/// <summary>
		/// The ZIndex of the text.
		/// Default: 0
		/// </summary>
		public int ZIndex { get; set; }

		/// <summary>
		/// Whether to center the text.
		/// </summary>
		public bool Center {
			get { return _center; }
			set {
				_center = value;
				CenterText();
			}
		}

		Vector2 size;
		bool sizeDirty;
		bool _center;

		/// <summary>
		/// Intialize a new instance of type Text
		/// </summary>
		/// <param name="newFilename">The filename (from the Content project) of the SpriteFont</param>
		/// <param name="newText">The string to be drawn</param>
		/// <param name="newZIndex">The ZIndex of the text</param>
		/// <param name="newCenter">Whether to center the text</param>
		public Text(string newFilename, string newText, int newZIndex = 0, bool newCenter = false) {
			filename = newFilename;
			_text = newText;
			Position = new Vector2();
			Color = Color.Black;
			Rotation = 0;
			Origin = new Vector2();
			Scale = new Vector2(1);
			Effects = SpriteEffects.None;
			LayerDepth = 0;
			ZIndex = newZIndex;
			Center = newCenter;

			sizeDirty = true;
		}

		/// <summary>
		/// Intialize a new instance of type Text
		/// </summary>
		/// <param name="newFilename">The filename (from the Content project) of the SpriteFont</param>
		/// <param name="newText">The string to be drawn</param>
		/// <param name="newPosition">The position of the text</param>
		/// <param name="newZIndex">The ZIndex of the text</param>
		/// <param name="newCenter">Whether to center the text</param>
		public Text(string newFilename, string newText, Vector2 newPosition, int newZIndex = 0, bool newCenter = false)
			: this (newFilename, newText, newZIndex, newCenter) {
			Position = newPosition;
		}

		/// <summary>
		/// Intialize a new instance of type Text
		/// </summary>
		/// <param name="newFilename">The filename (from the Content project) of the SpriteFont</param>
		/// <param name="newText">The string to be drawn</param>
		/// <param name="newPosition">The position of the text</param>
		/// <param name="newColor">The color of the text</param>
		/// <param name="newZIndex">The ZIndex of the text</param>
		/// <param name="newCenter">Whether to center the text</param>
		public Text(string newFilename, string newText, Vector2 newPosition, Color newColor, int newZIndex = 0, bool newCenter = false)
			: this(newFilename, newText, newZIndex, newCenter) {
			Position = newPosition;
			Color = newColor;
		}

		/// <summary>
		/// Intitialize a new instance of type Text as a copy of another Text
		/// </summary>
		/// <param name="other">The Text to copy drawing parameters from</param>
		public Text(Text other) {
			filename = other.filename;
			spriteFont = other.spriteFont;
			_text = other._text;
			Position = other.Position;
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

		/// <summary>
		/// Loads the SpriteFont of the text
		/// </summary>
		/// <param name="contentManager">The ContentManager</param>
		public void LoadContent(ContentManager contentManager) {
			spriteFont = contentManager.Load<SpriteFont>(filename);
		}

		/// <summary>
		/// Draws the text
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch</param>
		public void Draw(SpriteBatch spriteBatch) {
			if (_center) {
				CenterText();
			}
			spriteBatch.DrawString(spriteFont, _text, Position, Color, Rotation, Origin, Scale, Effects, LayerDepth);
		}

		void CenterText() {
			if (sizeDirty) {
				CleanSize();
			}
			Origin = _center ? (size / 2) : new Vector2();
		}

		void CleanSize() {
			size = spriteFont.MeasureString(_text);
			sizeDirty = false;
		}
	}
}