using ArchGame.Components.XnaComponents;
using ArchGame.States;
using Microsoft.Xna.Framework;

namespace Test.States {
	public class LoadingState : State {
		 public LoadingState() {
			 componentList.Add(new Text("font", "Please wait, loading...", new Vector2(10, 10), Color.White));
		 }
	}
}