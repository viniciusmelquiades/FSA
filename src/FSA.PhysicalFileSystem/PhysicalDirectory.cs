using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FSA.PhysicalFileSystem
{
	public class PhysicalDirectory : BaseDirectory
	{
		private readonly PhysicalFileSystem _fs;

		internal PhysicalDirectory(PhysicalFileSystem fs)
			: this(fs, "/")
		{ }

		protected internal PhysicalDirectory(PhysicalFileSystem fs, string path)
			: base(fs, path)
		{
			_fs = fs;

			FullPath = fs.GetFullPath(path);
		}

		public string FullPath { get; private set; }
		
		protected override void CreateDirectory()
		{
			Directory.CreateDirectory(FullPath);
		}

		protected override void DeleteDirectory()
		{
			Directory.Delete(FullPath);
		}

		protected override void MoveDirectory(IDirectory targetDirectory)
		{
			var physicalDestinationPath = _fs.GetFullPath(targetDirectory.Path);

			Directory.Move(FullPath, physicalDestinationPath);
		}

		protected override IDirectory GetParent()
		{
			return new PhysicalDirectory(_fs, System.IO.Path.GetDirectoryName(Path));
		}

		protected override IEnumerable<IFile> GetFilesInternal()
		{
			return Directory.EnumerateFiles(FullPath).Select(file => new PhysicalFile(_fs, PathUtil.Resolve(Path, file)));
		}

		protected override IEnumerable<IDirectory> GetDirectoriesInternal()
		{
			return Directory.EnumerateDirectories(FullPath).Select(directory => new PhysicalDirectory(_fs, PathUtil.Resolve(Path, directory)));
		}

		public override bool Exists
		{
			get { return Directory.Exists(FullPath); }
		}

		public override IFile GetFile(string path)
		{
			return new PhysicalFile(_fs, PathUtil.Resolve(path, path));
		}

		public override IDirectory GetDirectory(string path)
		{
			return new PhysicalDirectory(_fs, PathUtil.Resolve(Path, path));
		}
	}
}