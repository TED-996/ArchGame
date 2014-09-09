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
	/// <summary>
	/// This is the base class for your game.
	/// Read the wiki for more info.
	/// </summary>
	public abstract class ArchGame : Game {
		LoadableSet loadableSet;

		/// <summary>
		/// The game's Logger. This class logs messages to the console or to a file.
		/// </summary>
		protected readonly ILogger logger;

		/// <summary>
		/// The game's InputManager. This class manages keyboard and mouse input.
		/// </summary>
		protected readonly InputManager inputManager;

		/// <summary>
		/// The game's ModuleFactory. This class construct, loads, fulfills the requests of and disposes objects at the appropriate time.
		/// </summary>
		protected readonly ModuleFactory moduleFactory;

		/// <summary>
		/// The game's StateManager. This class maintains the state stack.
		/// </summary>
		protected StateManager stateManager;

		/// <summary>
		/// The game's GraphicsDevice. This class, along with the SpriteBatch, takes care of the rendering to the screen.
		/// </summary>
		protected readonly GraphicsDeviceManager graphicsDevice;

		/// <summary>
		/// The game's SpriteBatch. This class, along with the GraphicsDeviceManager, takes care of the rendering to the screen.
		/// </summary>
		protected SpriteBatch spriteBatch;

		/// <summary>
		/// Initialize the base class of a ArchGame implementation.
		/// </summary>
		/// <param name="gameName">The name of the game window</param>
		/// <param name="screenWidth"></param>
		/// <param name="screenHeight"></param>
		/// <param name="newLogger">The logger to use. If null, a NullLogger will be used.</param>
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

			logger = newLogger ?? new NullLogger();
			inputManager = new InputManager();

			moduleFactory = new ModuleFactory();

			moduleFactory.RegisterProvider(inputManager);

			ClipboardManager.Initialize(Window);
			EventInputManager.Initialize(Window);
			AppDomain.CurrentDomain.UnhandledException += OnException;
		}

		/// <summary>
		/// Initialize a new Game object.
		/// The logger will be set to a ThreadedLogger.
		/// </summary>
		/// <param name="gameName">The name of the game window</param>
		/// <param name="screenWidth"></param>
		/// <param name="screenHeight"></param>
		/// <param name="contentRoot">Name of the content root folder</param>
		/// <param name="fullscreen">Whether the game should be full-screen</param>
		protected ArchGame(string gameName, int screenWidth, int screenHeight, string contentRoot = "Content",
			bool fullscreen = false) : this(gameName, screenWidth, screenHeight, new ThreadedLogger(), contentRoot, fullscreen) {
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
			moduleFactory.RegisterProvider(stateManager);

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
			moduleFactory.FulfillRequests();
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
			inputManager.Update(Keyboard.GetState(), Mouse.GetState(), gameTime);

			stateManager.ObstructArea(inputManager);

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

		/// <summary>
		/// Logs the exception if one occurs.
		/// </summary>
		void OnException(object sender, UnhandledExceptionEventArgs e) {
			Exception exception = e.ExceptionObject as Exception;
			if (exception == null) {
				logger.Log("Unhandled, unknown exception occured. Things have probably gone very wrong.");
			}
			else {
				logger.Log(exception);
			}
		}
	}
}