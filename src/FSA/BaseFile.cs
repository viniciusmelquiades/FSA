using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSA
{
	public abstract class BaseFile : IFile
	{
		private IDirectory _directory;
		private IFileSystem _fs;

		protected internal BaseFile(IFileSystem fs, string path)
		{
			_fs = fs;
			Path = path;

			Name = System.IO.Path.GetFileName(path);
			NameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(path);
			Extension = System.IO.Path.GetExtension(path);
		}

		public string Path { get; private set; }

		public string Name { get; private set; }

		public string NameWithoutExtension { get; private set; }

		public string Extension { get; private set; }

		public abstract bool Exists { get; }

		public IDirectory Directory
		{
			get
			{
				if(_directory == null)
					_directory = GetDirectory();

				return _directory;
			}
		}

		public IFile Move(string path, bool replace = false)
		{
			var newFile = PrepareToMoveOrCopyFile(path, replace);

			MoveFile(newFile);

			return newFile;
		}
		
		public IFile Copy(string path, bool replace = false)
		{
			var newFile = PrepareToMoveOrCopyFile(path, replace);

			CopyFile(newFile);

			return newFile;
		}

		public void Delete()
		{
			if(Exists)
				DeleteFile();
		}

		public abstract System.IO.Stream OpenRead();

		public System.IO.Stream OpenWrite(bool append = true)
		{
			Directory.Create();

			if(append == false)
				Delete();

			return GetWriteStream();
		}

		//Private methods
		private IFile PrepareToMoveOrCopyFile(string path, bool replace)
		{
			var newFile = _fs.GetFile(PathUtil.Resolve(Path, path));

			if(newFile.Exists)
			{
				if(replace)
					newFile.Delete();
				else
					throw new DestinationExistsException();
			}

			newFile.Directory.Create();
			return newFile;
		}

		//Abstract methods
		protected abstract IDirectory GetDirectory();

		protected abstract void MoveFile(IFile newFile);

		protected abstract void CopyFile(IFile newFile);

		protected abstract void DeleteFile();

		protected abstract System.IO.Stream GetWriteStream();
	}
}
