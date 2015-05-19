using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSA
{
	public abstract class BaseDirectory : IDirectory
	{
		private IDirectory _parent;
		private readonly IFileSystem _fs;

		protected internal BaseDirectory(IFileSystem fs, string path)
		{
			_fs = fs;
			Name = path.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
		}

		public string Path { get; private set; }

		public string Name { get; private set; }

		public abstract bool Exists { get; }

		public void Create()
		{
			if(!Exists)
			{
				CreateDirectory();
			}
		}

		public void Delete()
		{
			if(Exists)
			{
				DeleteDirectory();
			}
		}

		public IDirectory Move(string destination)
		{
			var targetDirectory = _fs.GetDirectory(PathUtil.Resolve(Path, destination));

			targetDirectory.Parent.Create();

			MoveDirectory(targetDirectory);

			return targetDirectory;
		}

		public IDirectory Parent
		{
			get
			{
				if(_parent == null)
					_parent = GetParent();

				return _parent;
			}
		}

		public abstract IFile GetFile(string path);

		public abstract IDirectory GetDirectory(string path);

		public IEnumerable<IFile> GetFiles()
		{
			if(!Exists)
				return Enumerable.Empty<IFile>();

			return GetFilesInternal();
		}

		public IEnumerable<IDirectory> GetDirectories()
		{
			if(!Exists)
				return Enumerable.Empty<IDirectory>();

			return GetDirectoriesInternal();
		}

		//Protected abstract methods
		protected internal abstract void CreateDirectory();

		protected internal abstract void DeleteDirectory();

		protected internal abstract void MoveDirectory(IDirectory targetDirectory);

		protected internal abstract IDirectory GetParent();

		protected internal abstract IEnumerable<IFile> GetFilesInternal();

		protected internal abstract IEnumerable<IDirectory> GetDirectoriesInternal();
	}
}
