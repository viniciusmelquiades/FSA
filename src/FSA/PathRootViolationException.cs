﻿using System;

namespace FSA
{
	public class PathRootViolationException : Exception
	{
		public PathRootViolationException()
			: this("Tried to access a path above the root of the FileSystem.")
		{ }

		public PathRootViolationException(string message)
			: base(message)
		{ }

		public PathRootViolationException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}