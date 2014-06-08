using SFMLGame;
using SFMLGame.XML;

namespace SFMLGameTest {
	public class XMLTester : XMLAble {
		public int Int1 = 100;
		public string String1 = "Hello wolrd!BAD";
		public int Int2 = 100;
		public ILogger Logger;

		public XMLTester() : base("XMLTester.xml") {
		}

		public override string ToString() {
			return "Int1: " + Int1 + "; String1: " + String1 + "; Int2: " + Int2;
		}
	}
}