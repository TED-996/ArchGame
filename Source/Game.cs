using System;
using System.Threading;
using SFML.Window;
using SFMLGame.Modules;
using SFMLGame.Services;
using Spooker.Content;
using Spooker.Core;

namespace SFMLGame {
	public abstract class Game : GameWindow {
		readonly LoadableSet loadableSet;

		protected readonly ILogger logger;

		protected readonly ModuleFactory moduleFactory;

		/// <summary>
		/// Initialize a new Game object.
		/// </summary>
		/// <param name="gameName">The name of the game window</param>
		/// <param name="framerate">The maximum framerate. Set to -1 to not limit it.</param>
		/// <param name="newLoadableSet">The LoadableSet of ILoadables to load in the LoadContent stage.</param>
		/// <param name="screenWidth"></param>
		/// <param name="screenHeight"></param>
		/// <param name="contentRoot">Name of the content root folder</param>
		/// <param name="newLogger">The logger to use.</param>
		protected Game(string gameName, uint framerate, uint screenWidth, uint screenHeight, string contentRoot,
			LoadableSet newLoadableSet, ILogger newLogger = null)
			: base(
				new GameSettings {
					AntialiasingLevel = 4,
					BitsPerPixel = 32,
					ClearColor = Spooker.Graphics.Color.Black,
					ContentDirectory = contentRoot,
					DepthBits = 0,
					FramerateLimit = framerate,
					Height = screenHeight,
					Width = screenWidth,
					MajorVersion = 2,
					MinorVersion = 0,
					StencilBits = 0,
					Style = Styles.Default,
					TimeStep = 1000 / framerate,
					Title = gameName,
					VerticalSync = true,
					TimeStepCap = 20,
				}) {
			loadableSet = newLoadableSet;

			logger = newLogger ?? new ThreadedLogger();

			moduleFactory = new ModuleFactory();
		}

		protected virtual void SetServiceProviders() {
			ServiceLocator.AddProviders(ServiceLocator.DefaultProviders);

			ServiceLocator.SetService(logger);
			ServiceLocator.SetService(moduleFactory);
		}

		protected virtual void GetServices() {
			
		}

		protected virtual void AfterLoadContent() {
			
		}

		public override void LoadContent(ContentManager content) {
			SetServiceProviders();
			GetServices();
			content.AddLoaders(ContentProvider.Default);
			base.LoadContent(content);

			logger.Log("Started preloading content.", "Game");
			loadableSet.Preload(content);
			logger.Log("Ended preloading content.", "Game");

			Thread loaderThread = new Thread(LoaderThreadStart);
			loaderThread.Start(content);

			logger.Log("Game starting...", "Game");

		}
		
		void LoaderThreadStart(Object content) {
			logger.Log("Factory starting up");
			moduleFactory.ConstructModules();
			moduleFactory.FullfillRequests();
			logger.Log("Factory done.");

			logger.Log("Started threaded loading.", "Game");
			LoadThreaded((ContentManager) content);
			logger.Log("Ended threaded loading.", "Game");

			AfterLoadContent();
		}

		void LoadThreaded(ContentManager content) {
			loadableSet.Load(content);
		}

		public override void Dispose() {
			logger.Log("Closing.", "Game");
			base.Dispose();
			moduleFactory.Dispose();

			logger.Dispose();
		}
	}
}