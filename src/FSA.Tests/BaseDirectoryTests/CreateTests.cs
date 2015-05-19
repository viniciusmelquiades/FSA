using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSA.Tests.BaseDirectoryTests
{
	[TestFixture]
	public class CreateTests
	{
		[Test]
		public void WhenCreatingDirectory_ShouldCheckIfItExists()
		{
			var fsMock = new Mock<IFileSystem>();
			var dirMock = new Mock<BaseDirectory>(fsMock.Object, "/");

			var dir = dirMock.Object;

			dir.Create();

			dirMock.Verify(x => x.Exists, Times.Once());
		}

		[Test]
		public void Creating_IfDirectoryAlreadyExists_ShouldNotCreateIt()
		{
			var fsMock = new Mock<IFileSystem>();
			var dirMock = new Mock<BaseDirectory>(fsMock.Object, "/");
			dirMock.Setup(x => x.Exists).Returns(true);

			var dir = dirMock.Object;

			dir.Create();

			dirMock.Verify(x=>x.CreateDirectory(), Times.Never());
		}

		[Test]
		public void Creating_IfDirectoryDoesntExists_ShouldCreateIt()
		{
			var fsMock = new Mock<IFileSystem>();
			var dirMock = new Mock<BaseDirectory>(fsMock.Object, "/");
			dirMock.Setup(x => x.Exists).Returns(false);

			var dir = dirMock.Object;

			dir.Create();

			dirMock.Verify(x => x.CreateDirectory(), Times.Once());
		}
	}
}
