using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
	public static class ExceptionHelpers
	{
		public static void ThrowFormattedException(this Exception exception)
		{
			throw new ApplicationException(string.Format("{0}, InnerException = {1}", 
				exception.Message, (exception.InnerException != null ) ? exception.InnerException.Message : string.Empty ) );
		}

		public static void ThrowFormattedException(this string message, params object[] args)
		{
			throw new ApplicationException(string.Format( message , args ) );
		}
			
		/// <exception cref="NullReferenceException"></exception>
		public static T ThrowIfNull<T>(this T value, string variableName) where T : class
		{
			if (value == null)
		   {
				throw new NullReferenceException(string.Format("Value is Null: {0}", variableName));
			}

			return value;
		}

		/// <exception cref="NullReferenceException"></exception>
		public static T ThrowIfNullWithMessage<T>(this T value, string message, params string[] args) where T : class
		{
			if (value == null)
			{
				throw new NullReferenceException(string.Format( message, args));
			}

			return value;
		}
		/// <exception cref="NullReferenceException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static string ThrowIfNullOrWhiteSpace(this string value, string variableName)
		{
			ThrowIfNull(value, variableName);

			if (String.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException(string.Format("Value is Null or WhiteSpace: {0}", variableName));
			}
			return value;
		}

		/// <exception cref="NullReferenceException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static string ThrowIfNullOrWhiteSpaceWithMessage(this string value, string message, params string[] args)
		{
			ThrowIfNullWithMessage(value, message, args);

			if (String.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException(string.Format(message, args));
			}
			return value;
		}
	}
}
   