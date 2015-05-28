using System;

namespace FSA
{
	public class RootPathViolationException : Exception
	{
		public RootPathViolationException()
			: this("Attempt to access a path above the root of the FileSystem.")
		{ }

		public RootPathViolationException(string message)
			: base(message)
		{ }

		public RootPathViolationException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}