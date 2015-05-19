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
	public class CopyTests
	{
		/*
		 * for all tests here, the following dir structure will be used
		 * /a/b.txt
		 * /c -> may or may not exists
		 */


		[Test]
		public void OnCopyingFile_IfTheTargetDirectoryDontExists_ShouldCreateIt()
		{
			var dirMock = new Mock<IDirectory>();
			dirMock.Setup(x => x.Exists).Returns(true);

			var fsMock = new Mock<IFileSystem>();

			var fileMock = new Mock<BaseFile>(fsMock.Object, "/a/b.txt");
			fileMock.Setup(x => x.Exists).Returns(false);
			fileMock.Setup(x => x.GetDirectory()).Returns(dirMock.Object);

			var targetFile = new Mock<IFile>();
			fsMock.Setup(x => x.GetFile("/c/d.txt")).Returns(targetFile.Object);

			targetFile.Setup(x => x.Directory.Exists).Returns(false);

			var file = fileMock.Object;

			file.Copy("/c/d.txt");

			targetFile.Verify(x => x.Directory.Create(), Times.Once());
		}

		[Test]
		public void OnCopyingFile_IfTheTargetFileAlreadyExists_AndParaterToReplaceItIsTrue_ShouldDeleteTargetFileBeforeCopying()
		{
			var dirMock = new Mock<IDirectory>();
			dirMock.Setup(x => x.Exists).Returns(true);

			var fsMock = new Mock<IFileSystem>();

			var fileMock = new Mock<BaseFile>(fsMock.Object, "/a/b.txt");
			fileMock.Setup(x => x.Exists).Returns(false);
			fileMock.Setup(x => x.GetDirectory()).Returns(dirMock.Object);

			var deletedTargetFile = false;

			var targetFile = new Mock<IFile>();
			fsMock.Setup(x => x.GetFile("/c/d.txt")).Returns(targetFile.Object);

			targetFile.Setup(x => x.Directory.Exists).Returns(true);
			targetFile.Setup(x => x.Exists).Returns(true);
			targetFile.Setup(x => x.Delete()).Callback(() => deletedTargetFile = true);

			var file = fileMock.Object;

			file.Copy("/c/d.txt", replace: true);

			fileMock.Setup(x => x.CopyFile(targetFile.Object)).Callback(() => deletedTargetFile.Should().Be(true, "because the user is replacing the target file"));
		}

		[Test]
		public void OnCopyingFile_IfTheTargetFileAlreadyExists_AndParaterToReplaceItIsFalse_ShouldThrowException()
		{
			var dirMock = new Mock<IDirectory>();
			dirMock.Setup(x => x.Exists).Returns(true);

			var fsMock = new Mock<IFileSystem>();

			var fileMock = new Mock<BaseFile>(fsMock.Object, "/a/b.txt");
			fileMock.Setup(x => x.Exists).Returns(false);
			fileMock.Setup(x => x.GetDirectory()).Returns(dirMock.Object);


			var targetFile = new Mock<IFile>();
			fsMock.Setup(x => x.GetFile("/c/d.txt")).Returns(targetFile.Object);

			targetFile.Setup(x => x.Directory.Exists).Returns(true);
			targetFile.Setup(x => x.Exists).Returns(true);

			var file = fileMock.Object;

			file.Invoking(x => x.Copy("/c/d.txt", replace: false)).ShouldThrow<DestinationExistsException>();
		}
	}
}
