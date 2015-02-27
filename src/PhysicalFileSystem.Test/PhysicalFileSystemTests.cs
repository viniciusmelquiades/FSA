using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace PhysicalFileSystem.Test
{
	[TestFixture]
	public class PhysicalFileSystemTests
	{
		[Test]
		public void CreatingFileSystemOnAExistingBaseDirectoryShouldPass()
		{
			var fs = new PhysicalFileSystem(Environment.CurrentDirectory, false);

			fs.Should().NotBeNull();
			fs.Root.Exists.Should().BeTrue();
		}

		[Test]
		public void CreatingFileSystemOnANonExistingBaseDirectoryShouldFail()
		{
			var path = Path.Combine(Environment.CurrentDirectory, "fakeFolder");
			try
			{
				var fs = new PhysicalFileSystem(path, false);

				Assert.Fail("Because the root directory doesn't exists, and it was specified not to be created");
			}
			catch(Exception ex)
			{
				ex.Should().BeOfType<System.IO.DirectoryNotFoundException>("Because the PhysicalFileSystem have failed to be created, as expected, because the root directory doesn't exist.");
			}
			finally
			{
				if(Directory.Exists(path))
					Directory.Delete(path);
			}
		}

		[Test]
		public void CreatingFileSystemOnANonExistingBaseDirectoryShouldPass()
		{
			var path = Path.Combine(Environment.CurrentDirectory, "fakeFolder");
			try
			{
				var fs = new PhysicalFileSystem(path, true);

				fs.Should().NotBeNull();
				fs.Root.Exists.Should().BeTrue();
			}
			finally
			{
				if(Directory.Exists(path))
					Directory.Delete(path);
			}
		}
	}
}
