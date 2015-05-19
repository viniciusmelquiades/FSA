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
	public class DeleteTests
	{
		[Test]
		public void WhenDeletingDirectory_ShouldCheckIfItExists()
		{
			var fsMock = new Mock<IFileSystem>();
			var dirMock = new Mock<BaseDirectory>(fsMock.Object, "/");

			var dir = dirMock.Object;

			dir.Delete();

			dirMock.Verify(x => x.Exists, Times.Once());
		}

		[Test]
		public void OnDeleting_IfDirectoryExists_ShouldDeleteIt()
		{
			var fsMock = new Mock<IFileSystem>();
			var dirMock = new Mock<BaseDirectory>(fsMock.Object, "/");
			dirMock.Setup(x => x.Exists).Returns(true);

			var dir = dirMock.Object;

			dir.Delete();

			dirMock.Verify(x => x.DeleteDirectory(), Times.Once());
		}

		[Test]
		public void OnDeleting_IfDirectoryDoesntExists_ShouldntDeleteIt()
		{
			var fsMock = new Mock<IFileSystem>();
			var dirMock = new Mock<BaseDirectory>(fsMock.Object, "/");
			dirMock.Setup(x => x.Exists).Returns(false);

			var dir = dirMock.Object;

			dir.Delete();

			dirMock.Verify(x => x.DeleteDirectory(), Times.Never());
		}
	}
}
