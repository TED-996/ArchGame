using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ArchGame {
	/// <summary>
	/// An ILogger is a class responsible for logging data to a file or the console.
	/// Implement this to use your own logger.
	/// </summary>
	public interface ILogger : IDisposable {
		/// <summary>
		/// Log a message with a configurable message, sender and message type.
		/// </summary>
		void Log(string newMessage, string newSender = null, LogMessageType newType = LogMessageType.Information);

		/// <summary>
		/// Log a message with a DateTime other than DateTime.Now and a configurable message, sender and message type.
		/// </summary>
		void Log(string newMessage, DateTime newDateTime, string newSender = null, LogMessageType newType = LogMessageType.Information);
		
		/// <summary>
		/// Log an exception.
		/// </summary>
		void Log(Exception exception);

		/// <summary>
		/// Log a preconstructed LogMessage.
		/// </summary>
		void Log(LogMessage message);
	}

	/// <summary>
	/// Logger that writes messages synchronously to a file.
	/// </summary>
	public class Logger : ILogger {
		protected readonly StreamWriter writer;

		const string Filename = "log.txt";

		/// <summary>
		/// Initialize a new instance of type Logger
		/// </summary>
		public Logger() {
			writer = new StreamWriter(Filename, false) { AutoFlush = false };
		}

		/// <summary>
		/// Log a message with a configurable message, sender and message type.
		/// </summary>
		public void Log(string newMessage, string newSender = null, LogMessageType newType = LogMessageType.Information) {
			Log(new LogMessage(newMessage, newSender, newType));
		}

		/// <summary>
		/// Log a message with a DateTime other than DateTime.Now and a configurable message, sender and message type.
		/// </summary>
		public void Log(string newMessage, DateTime newDateTime, string newSender = null,
			LogMessageType newType = LogMessageType.Information) {
			Log(new LogMessage(newMessage, newDateTime, newSender, newType));
		}

		/// <summary>
		/// Log an exception.
		/// </summary>
		public void Log(Exception exception) {
			Log(new LogMessage(exception));
		}

		/// <summary>
		/// Log a preconstructed LogMessage.
		/// </summary>
		public virtual void Log(LogMessage message) {
			writer.WriteLine(message.ToString());
			writer.Flush();
		}

		/// <summary>
		/// Dispose of the writer.
		/// </summary>
		public virtual void Dispose() {
			writer.Dispose();
		}
	}

	/// <summary>
	/// Logger that writes messages asynchronously to a file.
	/// The messages are saved to a queue and are written every 0.1 seconds.
	/// </summary>
	public class ThreadedLogger : Logger {
		const int MaxDequeueTime = 50;
		const int ThreadSleepTime = 100;

		readonly Queue<LogMessage> messages;
		readonly Thread loggerThread;
		bool isRunningSlowly;
		volatile bool shouldThreadTerminate;

		/// <summary>
		/// Initializes a new instance of type ThreadedLogger.
		/// </summary>
		public ThreadedLogger() {
			messages = new Queue<LogMessage>();
			loggerThread = new Thread(ThreadRoot);
			loggerThread.Start();
			shouldThreadTerminate = false;
		}

		/// <summary>
		/// Empties and writes the message queue every 0.1 seconds.
		/// </summary>
		void ThreadRoot() {
			try {
				Stopwatch stopwatch = new Stopwatch();
				if (Sleep()) {
					return;
				}
				PrintMessage(new LogMessage("Threaded logging started.", "Logger"));
				while (true) {
					stopwatch.Start();
					PrintQueue();
					stopwatch.Stop();
					if (stopwatch.ElapsedMilliseconds > MaxDequeueTime && !isRunningSlowly) {
						PrintMessage(new LogMessage("Logger running slowly.", "Logger", LogMessageType.Warning));
						isRunningSlowly = true;
					}
					else {
						isRunningSlowly = false;
					}
					writer.Flush();
					if (Sleep()) {
						return;
					}
				}
			}
			finally {
				try {
					PrintQueue();
					PrintMessage(new LogMessage("Logging ended. Application closing.", "Logger"));
				}
				finally {
					writer.Dispose();
				}
			}
		}

		/// <summary>
		/// Empties and writes the queue.
		/// </summary>
		void PrintQueue() {
			while (messages.Count != 0) {
				LogMessage message;
				lock (messages) {
					message = messages.Dequeue();
				}
				PrintMessage(message);
			}
		}

		/// <summary>
		/// Sleeps a certain number of seconds, aborting if the thread has been signaled to quit.
		/// </summary>
		/// <returns>True if the thread should abort.</returns>
		bool Sleep() {
			const int checkNumber = 10;
			const int millisecondsBetweenChecks = ThreadSleepTime / checkNumber;
			for (int i = 0; i < checkNumber; i++) {
				if (shouldThreadTerminate) {
					return true;
				}
				Thread.Sleep(millisecondsBetweenChecks);
			}
			return false;
		}

		/// <summary>
		/// Writes a message to the file.
		/// </summary>
		void PrintMessage(LogMessage message) {
			writer.WriteLine(message.ToString());
		}

		/// <summary>
		/// Enqueues a LogMessage.
		/// </summary>
		public override void Log(LogMessage message) {
			lock (messages) {
				messages.Enqueue(message);
			}
		}

		/// <summary>
		/// Closes the logger thread.
		/// </summary>
		public override void Dispose() {
			shouldThreadTerminate = true;

			const int checkNumber = 10;
			const int millisecondsBetweenChecks = 2 * ThreadSleepTime / checkNumber;
			for (int i = 0; i < checkNumber; i++) {
				if (!loggerThread.IsAlive) {
					return;
				}
				Thread.Sleep(millisecondsBetweenChecks);
			}
			if (!loggerThread.IsAlive) {
				return;
			}
			loggerThread.Abort(); //Forcibly close it if necesarry.
		}
	}

	/// <summary>
	/// Logger that writes messages synchronously to the console.
	/// </summary>
	public class ConsoleLogger : ILogger {
		/// <summary>
		/// Log a message with a configurable message, sender and message type.
		/// </summary>
		public void Log(string newMessage, string newSender = null, LogMessageType newType = LogMessageType.Information) {
			Log(new LogMessage(newMessage, newSender, newType));
		}

		/// <summary>
		/// Log a message with a DateTime other than DateTime.Now and a configurable message, sender and message type.
		/// </summary>
		public void Log(string newMessage, DateTime newDateTime, string newSender = null,
			LogMessageType newType = LogMessageType.Information) {
			Log(new LogMessage(newMessage, newDateTime, newSender, newType));
		}

		/// <summary>
		/// Log an exception.
		/// </summary>
		public void Log(Exception exception) {
			Log(new LogMessage(exception));
		}

		/// <summary>
		/// Logs a preconstructed LogMessage to the console.
		/// </summary>
		/// <param name="message"></param>
		public void Log(LogMessage message) {
			Console.WriteLine(message.ToString());
		}

		/// <summary>
		/// Disposes the ConsoleLogger
		/// </summary>
		public void Dispose() {

		}
	}

	/// <summary>
	/// Mock Logger, doesn't log any messages. Use this when you want to disable logging.
	/// </summary>
	public class NullLogger : ILogger {
		/// <summary>
		/// Doesn't log anything.
		/// </summary>
		public void Log(string newMessage, string newSender = null, LogMessageType newType = LogMessageType.Information) {
			
		}

		/// <summary>
		/// Doesn't log anything.
		/// </summary>
		public void Log(string newMessage, DateTime newDateTime, string newSender = null,
			LogMessageType newType = LogMessageType.Information) {
			
		}

		/// <summary>
		/// Doesn't log anything.
		/// </summary>
		public void Log(Exception exception) {
			
		}

		/// <summary>
		/// Doesn't log anything.
		/// </summary>
		public void Log(LogMessage message) {
			
		}

		/// <summary>
		/// Doesn't dispose anything.
		/// </summary>
		public void Dispose() {
			
		}
	}

	/// <summary>
	/// LogMessage is a struct that keeps data about a logger message.
	/// </summary>
	public struct LogMessage {
		/// <summary>
		/// The date and time of the log message.
		/// </summary>
		public readonly DateTime DateTime;

		/// <summary>
		/// The message type.
		/// </summary>
		public readonly LogMessageType MessageType;

		/// <summary>
		/// The sender's name
		/// </summary>
		public readonly string Sender;

		/// <summary>
		/// The message to be written.
		/// </summary>
		public readonly string Message;

		/// <summary>
		/// Initialize a new instance of type LogMessage with a configurable message, sender and message type.
		/// The time is considered to be DateTime.Now.
		/// </summary>
		public LogMessage(string newMessage, string newSender = null,
			LogMessageType newType = LogMessageType.Information) {
			DateTime = DateTime.Now;
			Message = newMessage;
			Sender = newSender;
			MessageType = newType;
		}

		/// <summary>
		/// Initialize a new instance of type LogMessage with a configurable message, DateTime, sender and message type.
		/// </summary>
		public LogMessage(string newMessage, DateTime newDateTime, string newSender = null,
						  LogMessageType newType = LogMessageType.Information) {
			DateTime = newDateTime;
			Message = newMessage;
			Sender = newSender;
			MessageType = newType;
		}

		/// <summary>
		/// Initialize a new instance for type LogMessage from an exception.
		/// </summary>
		/// <param name="exception"></param>
		public LogMessage(Exception exception) {
			DateTime = DateTime.Now;
			Message = exception.Message + "\nStacktrace:\n" + exception.StackTrace;
			Sender = exception.TargetSite.Name;
			MessageType = LogMessageType.Error;
		}

		/// <summary>
		/// Convent a LogMessage to a string.
		/// </summary>
		public override string ToString() {
			return "[" + DateTime + ", " + (MessageType == LogMessageType.Information ? "INFO" :
				(MessageType == LogMessageType.Warning ? "WARNING" : "ERROR")) + ", " + (Sender ?? "Game") + "]: " + Message;
		}
	}

	/// <summary>
	/// This enum represents the type of the message that will be logged by the application's logger.
	/// </summary>
	public enum LogMessageType {
		/// <summary>
		/// This message is an information. It is only a trace of events and does not signal a problem.
		/// </summary>
		Information,
		/// <summary>
		/// This message signals a potential problem.
		/// </summary>
		Warning,
		/// <summary>
		/// This message signals a severe problem.
		/// </summary>
		Error
	}
}