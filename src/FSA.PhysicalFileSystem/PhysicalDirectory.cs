using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FSA.PhysicalFileSystem
{
	public class PhysicalDirectory : IDirectory
	{
		private readonly PhysicalFileSystem _fs;
		private PhysicalDirectory _parent;
		private string _path;

		internal PhysicalDirectory(PhysicalFileSystem fs)
			: this(fs, "/")
		{ }

		protected internal PhysicalDirectory(PhysicalFileSystem fs, string path)
		{
			_fs = fs;
			_path = path;

			FullPath = fs.GetFullPath(path);
			Name = FullPath.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries).Last();
		}

		public string FullPath
		{ get; private set; }

		public string Name
		{ get; private set; }

		public bool Exists
		{
			get
			{ return Directory.Exists(FullPath); }
		}

		public void Create()
		{
			if(!Exists)
			{
				Directory.CreateDirectory(FullPath);
			}
		}

		public void Delete()
		{
			if(Exists)
			{
				Directory.Delete(FullPath, true);
			}
		}

		public void Move(string destination)
		{
			Directory.Move(FullPath, _fs.GetFullPath(_path, destination));
		}

		public IDirectory Parent
		{
			get
			{
				if(_parent == null)
					_parent = new PhysicalDirectory(_fs, Path.GetDirectoryName(_path));

				return _parent;
			}
		}

		public IFile GetFile(string path)
		{
			return new PhysicalFile(_fs, PathUtil.Resolve(_path, path));
		}

		public IDirectory GetDirectory(string path)
		{
			return new PhysicalDirectory(_fs, PathUtil.Resolve(_path, path));
		}

		public IEnumerable<IFile> GetFiles()
		{
			if(!Exists)
				return Enumerable.Empty<IFile>();

			return Directory.EnumerateFiles(FullPath).Select(file => new PhysicalFile(_fs, PathUtil.Resolve(_path, file)));
		}

		public IEnumerable<IDirectory> GetDirectories()
		{
			if(!Exists)
				return Enumerable.Empty<IDirectory>();

			return Directory.EnumerateDirectories(FullPath).Select(directory => new PhysicalDirectory(_fs, PathUtil.Resolve(_path, directory)));
		}
	}
}