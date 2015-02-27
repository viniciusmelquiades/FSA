using System.IO;

namespace FSA.PhysicalFileSystem
{
	public class PhysicalFile : IFile
	{
		public readonly PhysicalFileSystem _fs;
		public readonly string _path;
		private PhysicalDirectory _directory;

		protected internal PhysicalFile(PhysicalFileSystem fs, string path)
		{
			_fs = fs;
			_path = path;

			FullName = fs.GetFullPath(path);
			Name = Path.GetFileName(FullName);
			Extension = Path.GetExtension(FullName);
		}

		public string FullName
		{ get; private set; }

		public string Name
		{ get; private set; }

		public string Extension
		{ get; private set; }

		public bool Exists
		{
			get
			{ return File.Exists(FullName); }
		}

		public IDirectory Directory
		{
			get
			{
				if(_directory == null)
					_directory = new PhysicalDirectory(_fs, Path.GetDirectoryName(_path));

				return _directory;
			}
		}

		public IFile Move(string path, bool replace = false)
		{
			var newFilePath = PathUtil.Resolve(_path, path);

			var physicalDestination = _fs.GetFullPath(newFilePath);

			if(File.Exists(physicalDestination))
			{
				if(replace)
					File.Delete(physicalDestination);
				else
					throw new DestinationExistsException();
			}

			File.Move(FullName, physicalDestination);

			return new PhysicalFile(_fs, newFilePath);
		}

		public IFile Copy(string path, bool replace = false)
		{
			var newFilePath = PathUtil.Resolve(_path, path);

			var physicalDestination = _fs.GetFullPath(newFilePath);

			if(File.Exists(physicalDestination))
			{
				if(replace)
					File.Delete(physicalDestination);
				else
					throw new DestinationExistsException();
			}

			File.Copy(FullName, physicalDestination);

			return new PhysicalFile(_fs, newFilePath);
		}

		public void Delete()
		{
			if(File.Exists(FullName))
				File.Delete(FullName);
		}

		public Stream OpenRead()
		{
			return File.OpenRead(FullName);
		}

		public Stream OpenWrite(bool append = true)
		{
			if(append == false)
				Delete();

			return File.OpenWrite(FullName);
		}
	}
}