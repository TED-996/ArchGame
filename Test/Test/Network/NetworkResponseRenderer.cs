using ArchGame.Components;
using ArchGame.Components.XnaComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test.Network {
	/// <summary>
	/// Draws a response to the screen.
	/// This is an example of drawing something without using the ComponentList.
	/// An easier way to include the ComponentList will be made shortly.
	/// </summary>
	public class NetworkResponseRenderer : IArchUpdateable, IArchDrawable, IArchLoadable {
		readonly INetworkManager networkManager;
		readonly Text responseText;

		public int UpdatePriority { get { return -1; } }
		public int ZIndex { get { return 5; } }

		public NetworkResponseRenderer(INetworkManager newNetworkManager) {
			networkManager = newNetworkManager;
			responseText = new Text("font", GetResponseString(), new Vector2(10, 10), Color.White);
		}

		string GetResponseString() {
			return "Random number: " + networkManager.GetResponse();
		}

		/// <summary>
		/// Load the content of the Text, that is, the font.
		/// </summary>
		public void LoadContent(ContentManager contentManager) {
			responseText.LoadContent(contentManager);
		}

		/// <summary>
		/// Update the text with the newest response.
		/// </summary>
		public void Update(GameTime gameTime) {
			responseText.TextToDraw = GetResponseString();
		}

		/// <summary>
		/// Draw the Text with the response.
		/// </summary>
		public void Draw(SpriteBatch spriteBatch) {
			responseText.Draw(spriteBatch);
		}
	}
}