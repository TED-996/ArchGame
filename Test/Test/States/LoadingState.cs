using ArchGame.Components.XnaComponents;
using ArchGame.States;
using Microsoft.Xna.Framework;

namespace Test.States {
	/// <summary>
	/// This will be shown while the game is loading.
	/// Since we're not loading much, the average player on the average PC won't have time to see it.
	/// </summary>
	public class LoadingState : State {
		 public LoadingState() {
			 //During the LoadingState we only want to see a Text that tells the player to wait.
			 //The ComponentList will take care of loading and drawing it, we don't have to worry anymore.
			 componentList.Add(new Text("font", "Please wait, loading...", new Vector2(10, 10), Color.White));
		 }
	}
}