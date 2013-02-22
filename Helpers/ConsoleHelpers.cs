using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Helpers
{
	public static class ConsoleHelpers
	{
		public static void WriteInDifferentColor(string message, ConsoleColor color, bool writeLine = false, params object[] args)
		{
			ConsoleColor currColor = Console.ForegroundColor;
			Console.ForegroundColor = color;
			if (writeLine) Console.WriteLine(message, args);
			else Console.Write(message, args);
			Console.ForegroundColor = currColor;
		}

		public static void WriteLineInDifferentColor(string message, ConsoleColor color, params object[] args)
		{
			WriteInDifferentColor(message, color, true, args);
		}

		public static string ReadKeyTimeout( string message, int timeout )
		{
			ConsoleKeyInfo cki = new ConsoleKeyInfo();
			Console.Write("\n{0}", message);
			int time = 0;
			while (true)
			{
				if (Console.KeyAvailable == false)
				{
					if (time >= timeout) return string.Empty;
					Thread.Sleep(1000);
					time++;
				}

				if (!Console.KeyAvailable) continue;
				
				cki = Console.ReadKey(true);
				return cki.Key.ToString();
			}
		}
	}
}
