using ArchGame.Components;
using ArchGame.Components.XnaComponents;
using Microsoft.Xna.Framework;

namespace Test.Network {
	/// <summary>
	/// Draws a response to the screen.
	/// This is an example of an entity made with the ComponentListUser.
	/// </summary>
	public class NetworkResponseRenderer : ComponentListUser {
		readonly INetworkManager networkManager;
		readonly Text responseText;

		public NetworkResponseRenderer(INetworkManager newNetworkManager) : base(0, 5) {
			networkManager = newNetworkManager;
			responseText = new Text("font", GetResponseString(), new Vector2(10, 10), Color.White);
			
			//Add the response Text to the component list
			componentList.Add(responseText);
		}

		string GetResponseString() {
			return "Random number: " + networkManager.GetResponse();
		}

		/// <summary>
		/// Update the text with the newest response.
		/// </summary>
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			responseText.TextToDraw = GetResponseString();
		}

	}
}