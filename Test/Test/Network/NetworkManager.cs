using System;
using System.Net;
namespace Test.Network {
	/// <summary>
	/// This is the interface for our network manager.
	/// We define an interface because, at a later moment, we may want to test the game without filling RANDOM.ORG with requests.
	/// </summary>
	public interface INetworkManager : IDisposable {
		void Request();
		string GetResponse();
	}

	/// <summary>
	/// This is the network manager. It pulls a random number from the RANDOM.ORG server.
	/// RANDOM.ORG's website uses a HTTP API.
	/// </summary>
	public class NetworkManager : INetworkManager {
		string response;
		readonly WebClient webClient;
		const string Url = "http://www.random.org/integers/?num=1&min=1&max=100&col=1&base=10&format=plain&rnd=new";

		public NetworkManager() {
			webClient = new WebClient();
		}

		public void Request() {
			response = null;
			//Register the event for when the response is received
			webClient.DownloadStringCompleted += OnDownloadStringCompleted;
			//Request the response.
			webClient.DownloadStringAsync(new Uri(Url));
		}

		void OnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e) {
			if (e.Error != null) {
				response = "Error.";
			}
			else {
				response = e.Result;
			}
		}

		public string GetResponse() {
			//If no response has been received yet, return "Waiting..."
			return response ?? "Waiting...";
		}

		/// <summary>
		/// Dispose of the WebClient.
		/// This will be called automatically by the ModuleFactory.
		/// </summary>
		public void Dispose() {
			webClient.Dispose();
		}
	}
}