using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SFMLGame {
	public interface ILogger : IDisposable {
		void Log(string newMessage, string newSender = null, LogMessageType newType = LogMessageType.Information);
		void Log(string newMessage, DateTime newDateTime, string newSender = null,
			LogMessageType newType = LogMessageType.Information);
		void Log(Exception exception);
		void Log(LogMessage message);
	}

	public class Logger : ILogger {
		protected readonly StreamWriter writer;

		const string Filename = "log.txt";

		public Logger() {
			writer = new StreamWriter(Filename, false) { AutoFlush = false };
		}


		public void Log(string newMessage, string newSender = null, LogMessageType newType = LogMessageType.Information) {
			Log(new LogMessage(newMessage, newSender, newType));
		}

		public void Log(string newMessage, DateTime newDateTime, string newSender = null,
			LogMessageType newType = LogMessageType.Information) {
			Log(new LogMessage(newMessage, newDateTime, newSender, newType));
		}

		public void Log(Exception exception) {
			Log(new LogMessage(exception));
		}

		public virtual void Log(LogMessage message) {
			writer.WriteLine(message.ToString());
			writer.Flush();
		}

		public virtual void Dispose() {
			
		}
	}

	public class ThreadedLogger : Logger {
		const int MaxDequeueTime = 50;
		const int ThreadSleepTime = 100;

		readonly Queue<LogMessage> messages;
		readonly Thread loggerThread;
		bool isRunningSlowly;
		volatile bool shouldThreadTerminate;


		public ThreadedLogger() {
			messages = new Queue<LogMessage>();
			loggerThread = new Thread(ThreadRoot);
			loggerThread.Start();
			shouldThreadTerminate = false;
		}

		void ThreadRoot() {
			try {
				Stopwatch stopwatch = new Stopwatch();
				if (Sleep()) { //Let the object generation run.
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

		void PrintMessage(LogMessage message) {
			writer.WriteLine(message.ToString());
		}

		public override void Log(LogMessage message) {
			lock (messages) {
				messages.Enqueue(message);
			}
		}

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

	public class ConsoleLogger : ILogger {
		public void Log(string newMessage, string newSender = null, LogMessageType newType = LogMessageType.Information) {
			Log(new LogMessage(newMessage, newSender, newType));
		}

		public void Log(string newMessage, DateTime newDateTime, string newSender = null,
			LogMessageType newType = LogMessageType.Information) {
			Log(new LogMessage(newMessage, newDateTime, newSender, newType));
		}

		public void Log(Exception exception) {
			Log(new LogMessage(exception));
		}

		public void Log(LogMessage message) {
			Console.WriteLine(message.ToString());
		}

		public void Dispose() {

		}
	}

	public class NullLogger : ILogger {
		public void Log(string newMessage, string newSender = null, LogMessageType newType = LogMessageType.Information) {
			
		}

		public void Log(string newMessage, DateTime newDateTime, string newSender = null,
			LogMessageType newType = LogMessageType.Information) {
			
		}

		public void Log(Exception exception) {
			
		}

		public void Log(LogMessage message) {
			
		}

		public void Dispose() {
			
		}
	}

	public struct LogMessage {
		public readonly DateTime DateTime;
		public readonly LogMessageType MessageType;
		public readonly string Sender;
		public readonly string Message;

		public LogMessage(string newMessage, string newSender = null,
			LogMessageType newType = LogMessageType.Information) {
			DateTime = DateTime.Now;
			Message = newMessage;
			Sender = newSender;
			MessageType = newType;
		}

		public LogMessage(string newMessage, DateTime newDateTime, string newSender = null,
						  LogMessageType newType = LogMessageType.Information) {
			DateTime = newDateTime;
			Message = newMessage;
			Sender = newSender;
			MessageType = newType;
		}

		public LogMessage(Exception exception) {
			DateTime = DateTime.Now;
			Message = exception.Message + "\nStacktrace:\n" + exception.StackTrace;
			Sender = exception.TargetSite.Name;
			MessageType = LogMessageType.Error;
		}

		public override string ToString() {
			return "[" + DateTime + ", " + (MessageType == LogMessageType.Information ? "INFO" :
				(MessageType == LogMessageType.Warning ? "WARNING" : "ERROR")) + ", " + (Sender ?? "Game") + "]: " + Message;
		}
	}

	public enum LogMessageType {
		Information, Warning, Error
	}
}