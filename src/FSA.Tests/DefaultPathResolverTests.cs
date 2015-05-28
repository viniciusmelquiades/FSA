using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSA.Tests
{
	[TestFixture]
	public class DefaultPathResolverTests
	{
		[Test]
		public void ResolvingPathWithMultiplePaths_ShouldConcatAllOfThem()
		{
			var resolver = new DefaultPathResolver();

			var result = resolver.Resolve(new[] { "/", "path1/path2", "path3/path4" });

			result.Should().Be("/path1/path2/path3/path4");
		}

		[Test]
		public void ResolvingPathWithMultiplePaths_EvenIfOneOfPartsInTheMiddleStartsWithADirectorySeparator_ShouldConcatAllOfThem()
		{
			var resolver = new DefaultPathResolver();

			var result = resolver.Resolve(new[] { "/", "path1/path2", "/path3/path4" });

			result.Should().Be("/path1/path2/path3/path4");
		}

		[Test]
		public void ResolvingToParentDirectory_ShouldReturnParentPath()
		{
			var resolver = new DefaultPathResolver();

			var result = resolver.Resolve(new[] { "/path1/path2", "..", "path3/path4" });

			result.Should().Be("/path1/path3/path4");
		}

		[Test(Description="During one of the first tests, resolving to the root directory with .. would throw an exception. So this test ensures everything works.")]
		public void MovingUpToRootDirectory_ShouldReturnRootDirectory_AndNotFail()
		{
			var resolver = new DefaultPathResolver();

			var result = resolver.Resolve(new[] { "/path1", ".." });

			result.Should().Be("/");
		}

		[Test]
		public void ShouldIgnoreSingleDotsInPath()
		{
			var resolver = new DefaultPathResolver();

			var result = resolver.Resolve(new[] { "/", "/path1/path2/.", "./path3/path4/./path5" });

			result.Should().Be("/path1/path2/path3/path4/path5");
		}

		[Test]
		public void TryingToNavigateAboveTheRootDirectory_ShouldStayInRoot()
		{
			var resolver = new DefaultPathResolver(rootPathViolationBehavior: RootPathViolationBehavior.StayInRoot);

			var result = resolver.Resolve(new[] { "/", "path1", "../.." });

			result.Should().Be("/");
		}

		[Test]
		public void TryingToNavigateAboveTheRootDirectory_WhenBehaviorIsSetToThrowException_ShouldThrowException()
		{
			var resolver = new DefaultPathResolver(rootPathViolationBehavior: RootPathViolationBehavior.ThrowException);

			resolver.Invoking(r => r.Resolve(new[] { "/", "/path1", "../.." }))
				.ShouldThrow<RootPathViolationException>();
		}
	}
}
