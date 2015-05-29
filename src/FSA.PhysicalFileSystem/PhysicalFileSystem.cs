using System.IO;

namespace FSA.PhysicalFileSystem
{
	public class PhysicalFileSystem : IFileSystem
	{
		internal readonly DirectoryInfo _root;
		private readonly PhysicalDirectory _rootDirectory;

		public PhysicalFileSystem(DirectoryInfo root, bool create = true, IPathResolver pathResolver = null)
		{
			_root = root;

			if(!root.Exists)
				if(create)
					root.Create();
				else
					throw new DirectoryNotFoundException();

			_rootDirectory = new PhysicalDirectory(this);

			if(pathResolver == null)
			{
				pathResolver = new DefaultPathResolver(Path.DirectorySeparatorChar.ToString());
			}
		}

		public PhysicalFileSystem(string root, bool create = true, IPathResolver pathResolver = null) :
			this(new DirectoryInfo(root), create, pathResolver)
		{ }

		public IDirectory Root
		{
			get
			{ return _rootDirectory; }
		}

		public IFile GetFile(params string[] pathParts)
		{
			return new PhysicalFile(this, PathResolver.Resolve(pathParts));
		}

		public IDirectory GetDirectory(params string[] pathParts)
		{
			return new PhysicalDirectory(this, PathResolver.Resolve(pathParts));
		}

		public IPathResolver PathResolver
		{ get; private set; }

		/// <summary>
		/// Resolves the path to the physical path. The same used by the OS.
		/// </summary>
		/// <param name="paths"></param>
		/// <returns>The path on the disk for the "virtual path"</returns>
		public virtual string GetFullPath(params string[] paths)
		{
			var fsPath = PathResolver.Resolve(paths);

			return Path.Combine(_root.FullName, fsPath);
		}
	}
}