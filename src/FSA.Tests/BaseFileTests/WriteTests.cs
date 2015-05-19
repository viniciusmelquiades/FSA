using FluentAssertions;
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
	public class WriteTests
	{
		[Test]
		public void OnWritingFile_IfDirectoryDoesntExist_ShouldCreateIt()
		{
			var dirMock = new Mock<IDirectory>();
			dirMock.Setup(x => x.Exists).Returns(false);

			var fsMock = new Mock<IFileSystem>();
			var fileMock = new Mock<BaseFile>(fsMock.Object, "/a/b.txt");
			fileMock.Setup(x => x.GetDirectory()).Returns(dirMock.Object);

			var file = fileMock.Object;

			file.OpenWrite();

			dirMock.Verify(x => x.Create(), Times.Once());
		}

		[Test]
		public void OnWritingFile_IfItAlreadExist_AndPassFalseToTheAppendParameter_ShouldDeleteItBeforeOpeningIt()
		{
			var deletedFile = false;

			var dirMock = new Mock<IDirectory>();
			dirMock.Setup(x => x.Exists).Returns(true);

			var fsMock = new Mock<IFileSystem>();
			var fileMock = new Mock<BaseFile>(fsMock.Object, "/a/b.txt");
			fileMock.Setup(x => x.Exists).Returns(true);
			fileMock.Setup(x => x.GetDirectory()).Returns(dirMock.Object);
			
			fileMock.Setup(x => x.DeleteFile()).Callback(() => deletedFile = true);
			fileMock.Setup(x => x.GetWriteStream()).Callback(() => deletedFile.Should().Be(true, "because the file cannot be opened without deleting it because the user don't want to append it's contents"));
			
			var file = fileMock.Object;

			file.OpenWrite(append: false);
		}

		[Test]
		public void OnWritingFile_IfItAlreadExist_AndPassTrueToTheAppendParameter_ShouldntDeleteItBeforeOpeningIt()
		{
			var dirMock = new Mock<IDirectory>();
			dirMock.Setup(x => x.Exists).Returns(true);

			var fsMock = new Mock<IFileSystem>();
			var fileMock = new Mock<BaseFile>(fsMock.Object, "/a/b.txt");
			fileMock.Setup(x => x.Exists).Returns(true);
			fileMock.Setup(x => x.GetDirectory()).Returns(dirMock.Object);

			fileMock.Setup(x => x.DeleteFile()).Callback(() => Assert.Fail("because the user wants to append the fail in this scenario"));

			var file = fileMock.Object;

			file.OpenWrite(append: true);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void OnWritingFile_IfItDoesntExist_ShouldNeverTryToDeleteIt_BecauseItDoesntExists(bool append)
		{
			var dirMock = new Mock<IDirectory>();
			dirMock.Setup(x => x.Exists).Returns(true);

			var fsMock = new Mock<IFileSystem>();
			var fileMock = new Mock<BaseFile>(fsMock.Object, "/a/b.txt");
			fileMock.Setup(x => x.Exists).Returns(false);
			fileMock.Setup(x => x.GetDirectory()).Returns(dirMock.Object);

			fileMock.Setup(x => x.DeleteFile()).Callback(() => Assert.Fail("because file doesn't exits"));

			var file = fileMock.Object;

			file.OpenWrite(append: append);
		}
	}
}
