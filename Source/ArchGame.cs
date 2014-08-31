using System;
using System.Threading;
using ArchGame.Components;
using ArchGame.States;
using Microsoft.Xna.Framework;
using ArchGame.Modules;
using ArchGame.Services;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArchGame {
	public abstract class ArchGame : Game {
		LoadableSet loadableSet;

		protected readonly ILogger logger;

		protected readonly ModuleFactory moduleFactory;

		protected StateManager stateManager;

		protected readonly GraphicsDeviceManager graphicsDevice;
		protected SpriteBatch spriteBatch;

		/// <summary>
		/// Initialize a new Game object.
		/// </summary>
		/// <param name="gameName">The name of the game window</param>
		/// <param name="screenWidth"></param>
		/// <param name="screenHeight"></param>
		/// <param name="newLogger">The logger to use.</param>
		/// <param name="contentRoot">Name of the content root folder</param>
		/// <param name="fullscreen">Whether the game should be full-screen</param>
		protected ArchGame(string gameName, int screenWidth, int screenHeight, ILogger newLogger, string contentRoot = "Content",
			bool fullscreen = false) {
			graphicsDevice = new GraphicsDeviceManager(this) {
				IsFullScreen = fullscreen,
				PreferredBackBufferWidth = screenWidth,
				PreferredBackBufferHeight = screenHeight,
				SynchronizeWithVerticalRetrace = true
			};

			Window.Title = gameName;
			Content.RootDirectory = contentRoot;

			logger = newLogger ?? new ThreadedLogger();

			moduleFactory = new ModuleFactory();
		}

		protected override void Initialize() {
			base.Initialize();
		}

		protected abstract State GetLoadingState();
		protected abstract State GetAfterLoadState();

		protected virtual void SetServiceProviders() {
			ServiceLocator.AddProviders(ServiceLocator.DefaultProviders);

			ServiceLocator.SetService(logger);
		}

		protected virtual void GetServices() {

		}

		protected virtual LoadableSet GetLoadableSet() {
			return new LoadableSet(new IArchLoadable[0], new IArchLoadable[0]);
		}

		protected virtual void AfterLoadContent(ContentManager contentManager) {
			stateManager.ReplaceTopState(GetAfterLoadState());
		}

		protected override void LoadContent() {
			SetServiceProviders();
			GetServices();

			spriteBatch = new SpriteBatch(GraphicsDevice);

			logger.Log("Started preloading content.", "Game");
			loadableSet = GetLoadableSet();
			loadableSet.Preload(Content);

			stateManager = new StateManager(moduleFactory);
			stateManager.LoadContent(Content);

			State loadingState = GetLoadingState();
			if (loadingState != null) {
				stateManager.PushState(loadingState);
			} 

			logger.Log("Ended preloading content.", "Game");

			Thread loaderThread = new Thread(LoaderThreadStart);
			loaderThread.Start(Content);

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

			loadableSet.Discard();

			AfterLoadContent(Content);
		}

		void LoadThreaded(ContentManager content) {
			loadableSet.Load(content);

			moduleFactory.LoadContent(Content);
		}

		protected override void Update(GameTime gameTime) {
			stateManager.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin();
			stateManager.Draw(spriteBatch);
			spriteBatch.End();
		}

		protected override void UnloadContent() {
			logger.Log("Closing.", "Game");
			Content.Dispose();
			moduleFactory.Dispose();

			logger.Dispose();
		}
	}
}