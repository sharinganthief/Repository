using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Helpers
{
	public static class Reporting
	{
		private static DateTime Start { get; set; }
		private static DateTime End { get; set; }
		private static Stopwatch Timer = new Stopwatch();

		public static void StartTimer()
		{
			if (Timer.IsRunning) return;

			Timer.Reset();
			Timer.Start();
		}

		public static void StopTimer()
		{
			if( ! Timer.IsRunning) return;

			Timer.Stop();
		}

		public static TimeSpan GetTimeResult()
		{
			return Timer == null ? new TimeSpan() : Timer.Elapsed;
		}

		public static void WriteResultToConsole( string message)
		{
			StopTimer();
			try
			{
				string formattedMessage = string.Format(message, GetTimeResult());
				Console.WriteLine( formattedMessage);
			}
			catch (Exception exception)
			{
				Console.WriteLine("Error writing to console: [ {0} ]", exception.Message);
			}
			
		}

		public static void WriteStart(string startReadNumberFromFile)
		{
			Start = DateTime.Now;
			
			try
			{
				string formattedMessage = string.Format(startReadNumberFromFile, Start);
				Console.WriteLine( formattedMessage);
			}
			catch (Exception exception)
			{
				Console.WriteLine("Error writing to console: [ {0} ]", exception.Message);
			}
			StartTimer();
		}

				public static void WriteEnd(string startReadNumberFromFile)
		{
			End = DateTime.Now;

			try
			{
				string formattedMessage = string.Format(startReadNumberFromFile, End);
				Console.WriteLine( formattedMessage);
			}
			catch (Exception exception)
			{
				Console.WriteLine("Error writing to console: [ {0} ]", exception.Message);
			}
					StopTimer();
		}
	}
}
