using System;
using System.Threading;
using ArchGame.Components;
using ArchGame.Input;
using ArchGame.States;
using Microsoft.Xna.Framework;
using ArchGame.Modules;
using ArchGame.Services;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArchGame {
	public abstract class ArchGame : Game {
		LoadableSet loadableSet;

		protected readonly ILogger logger;
		protected readonly InputState inputState;

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
		/// <param name="newLogger">The logger to use. If null, a ThreadedLogger will be used.</param>
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
			inputState = new InputState();

			moduleFactory = new ModuleFactory();

			moduleFactory.RegisterProvider(inputState);

			ClipboardManager.Initialize(Window);
			EventInputManager.Initialize(Window);
			AppDomain.CurrentDomain.UnhandledException += OnException;
		}

		void OnException(object sender, UnhandledExceptionEventArgs e) {
			Exception exception = e.ExceptionObject as Exception;
			if (exception == null) {
				logger.Log("Unhandled, unknown exception occured. Things have probably gone very wrong.");
			}
			else {
				logger.Log(exception);
			}
		}

		/// <summary>
		/// Initialize a new Game object.
		/// </summary>
		/// <param name="gameName">The name of the game window</param>
		/// <param name="screenWidth"></param>
		/// <param name="screenHeight"></param>
		/// <param name="contentRoot">Name of the content root folder</param>
		/// <param name="fullscreen">Whether the game should be full-screen</param>
		protected ArchGame(string gameName, int screenWidth, int screenHeight, string contentRoot = "Content",
			bool fullscreen = false) : this(gameName, screenWidth, screenHeight, null, contentRoot, fullscreen) {
		}

		/// <summary>
		/// Get the first state to be shown on screen, the loading state, while the content is loaded.
		/// </summary>
		protected abstract State GetLoadingState();

		/// <summary>
		/// Gets the first state to be shown on screen after the content has been loaded.
		/// </summary>
		protected abstract State GetAfterLoadState();

		/// <summary>
		/// Sets the service providers and services.
		/// </summary>
		protected virtual void SetServiceProviders() {
			ServiceLocator.AddProviders(ServiceLocator.DefaultProviders);

			ServiceLocator.SetService(logger);
		}

		/// <summary>
		/// Will be overriden by implementations to get the necessary services after those have been initialized.
		/// </summary>
		protected virtual void SetServices() {

		}

		/// <summary>
		/// Returns the LoadableSet that 
		/// </summary>
		protected virtual LoadableSet GetLoadableSet() {
			return new LoadableSet(new IArchLoadable[0], new IArchLoadable[0]);
		}

		/// <summary>
		/// Will be called after the content has been loaded.
		/// </summary>
		/// <param name="contentManager">The ContentManager</param>
		protected virtual void AfterLoadContent(ContentManager contentManager) {
			stateManager.ReplaceTopState(GetAfterLoadState());
		}

		/// <summary>
		/// Preloads the content and starts the loading thread
		/// </summary>
		protected override void LoadContent() {
			SetServiceProviders();
			SetServices();

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
		
		/// <summary>
		/// Constructs the modules and loads the content on the loading thread
		/// </summary>
		/// <param name="content">The ContentManager</param>
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

		/// <summary>
		/// Loads the content on the loading thread
		/// </summary>
		/// <param name="content">The content manager</param>
		void LoadThreaded(ContentManager content) {
			loadableSet.Load(content);

			moduleFactory.LoadContent(Content);
		}

		/// <summary>
		/// Updates the game
		/// </summary>
		/// <param name="gameTime">The GameTime</param>
		protected override void Update(GameTime gameTime) {
			inputState.Update(Keyboard.GetState(), Mouse.GetState(), gameTime);
			stateManager.Update(gameTime);
		}

		/// <summary>
		/// Draws the game
		/// </summary>
		/// <param name="gameTime">The GameTime</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin();
			stateManager.Draw(spriteBatch);
			spriteBatch.End();
		}

		/// <summary>
		/// Unloads the game, disposing it.
		/// </summary>
		protected override void UnloadContent() {
			logger.Log("Closing.", "Game");
			Content.Dispose();
			moduleFactory.Dispose();

			logger.Dispose();
		}
	}
}