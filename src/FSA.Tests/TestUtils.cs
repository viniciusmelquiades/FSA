using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSA.Tests
{
	public static class TestUtils
	{
		public static Mock<IFileSystem> MockFileSystem()
		{
			return new Mock<IFileSystem>();
		}
	}
}
