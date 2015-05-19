using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSA.Tests.BaseFileTests
{
	[TestFixture]
	public class DeleteTests
	{
		[Test]
		public void WhenDeletingFile_ShouldCheckIfItExists()
		{
			var fsMock = new Mock<IFileSystem>();
			var fileMock = new Mock<BaseFile>(fsMock.Object, "/a.txt");

			var file = fileMock.Object;

			file.Delete();

			fileMock.Verify(x => x.Exists, Times.Once());
		}

		[Test]
		public void OnDeleting_IfFileExists_ShouldDeleteIt()
		{
			var fsMock = new Mock<IFileSystem>();
			var fileMock = new Mock<BaseFile>(fsMock.Object, "/a.txt");
			fileMock.Setup(x => x.Exists).Returns(true);

			var file = fileMock.Object;

			file.Delete();

			fileMock.Verify(x => x.DeleteFile(), Times.Once());
		}

		[Test]
		public void OnDeleting_IfFileDoesntExists_ShouldntDeleteIt()
		{
			var fsMock = new Mock<IFileSystem>();
			var fileMock = new Mock<BaseFile>(fsMock.Object, "/a.txt");
			fileMock.Setup(x => x.Exists).Returns(false);

			var file = fileMock.Object;

			file.Delete();

			fileMock.Verify(x => x.DeleteFile(), Times.Never());
		}
	}
}
