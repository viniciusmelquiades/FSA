using System.IO;

namespace FSA.PhysicalFileSystem
{
	public class PhysicalFileSystem : IFileSystem
	{
		internal readonly DirectoryInfo _root;
		private readonly PhysicalDirectory _rootDirectory;

		public PhysicalFileSystem(DirectoryInfo root, bool create = true)
		{
			_root = root;

			if(!root.Exists)
				if(create)
					root.Create();
				else
					throw new DirectoryNotFoundException();

			_rootDirectory = new PhysicalDirectory(this);
		}

		public PhysicalFileSystem(string root, bool create = true) :
			this(new DirectoryInfo(root), create)
		{ }

		public IDirectory Root
		{
			get
			{ return _rootDirectory; }
		}

		public IFile GetFile(string path)
		{
			return _rootDirectory.GetFile(path);
		}

		public IDirectory GetDirectory(string path)
		{
			return _rootDirectory.GetDirectory(path);
		}

		/// <summary>
		/// Resolves the paths, using <see cref="PathUtil.Resolve(char, string[])"/>, and combines it, with <see cref="Path.Combine(string, string)"/> to generate the physical path of a file.
		/// </summary>
		/// <param name="paths"></param>
		/// <returns>The path on the disk for the "virtual path"</returns>
		public virtual string GetFullPath(params string[] paths)
		{
			var fsPath = PathUtil.Resolve(Path.DirectorySeparatorChar, paths);

			return Path.Combine(_root.FullName, fsPath);
		}
	}
}