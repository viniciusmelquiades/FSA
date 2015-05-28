using FluentAssertions;
using NUnit.Framework;

namespace FSA.Tests.PathUtilTests
{
	[TestFixture]
	[System.Obsolete]
	public class ResolveTests
	{
		[Test]
		public void ResolveShouldUseWhateverSeparatorWeWant()
		{
			const string testPath = @"/path1/\path2\path3";

			PathUtil.ResolveWithSeparator("/", testPath).Should().Be("/path1/path2/path3");
			PathUtil.ResolveWithSeparator("\\", testPath).Should().Be(@"\path1\path2\path3");
			PathUtil.ResolveWithSeparator("-", testPath).Should().Be("-path1-path2-path3");
			PathUtil.ResolveWithSeparator("@", testPath).Should().Be("@path1@path2@path3");
			PathUtil.ResolveWithSeparator("%", testPath).Should().Be("%path1%path2%path3");
		}

		[Test]
		public void ResolveBeingCalledWithManyPaths_NotStartingWithDirectorySeparators_ShouldConcatAllOfThem()
		{
			PathUtil.ResolveWithSeparator("/", "path1/path2", "path3/path4").Should().Be("/path1/path2/path3/path4");
		}

		[Test]
		public void ResolveBeingCalledWithManyPaths_HavingPathStartingWithDirectorySeparator_ShouldNotConcatPathsBeforeSeparator()
		{
			PathUtil.ResolveWithSeparator("/", "path1/path2", "/path3/path4").Should().Be("/path3/path4");
			PathUtil.ResolveWithSeparator("/", "path1/path2", "\\path3/path4").Should().Be("/path3/path4");
		}

		[Test]
		public void ResolveBeingCalledToParentDirectory_ShouldReturnParentPath()
		{
			PathUtil.ResolveWithSeparator("/", "/path1/path2", "..").Should().Be("/path1");
			PathUtil.ResolveWithSeparator("/", "path1/path2/path3", "..").Should().Be("/path1/path2");
		}

		[Test]
		public void ShouldBeAbleRoResolveToRootPath()
		{
			PathUtil.ResolveWithSeparator("/", "/path1", "..").Should().Be("/");
			PathUtil.ResolveWithSeparator("/", "/path1/path2", "../..").Should().Be("/");
		}

		[Test]
		public void TryingToResolvePathAboveRoot_ShouldThrowException()
		{
			Assert.Throws(typeof(RootPathViolationException), () => PathUtil.ResolveWithSeparator("/", "/path1", "../.."));
		}

		[Test]
		public void PathPartsThatEqualsDot_ShouldBeIgnored()
		{
			PathUtil.ResolveWithSeparator("/", "/path1/path2/.", "./path3/path4/./path5").Should().Be("/path1/path2/path3/path4/path5");
		}
	}
}