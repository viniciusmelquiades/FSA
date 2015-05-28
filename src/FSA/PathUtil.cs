using System;
using System.IO;
using System.Linq;

namespace FSA
{
	[Obsolete("Use one of the IPathResolver. The DefaultPathResolver will probably be ok for EVERYTHING. This class will fail to compile in the future before being removed.")]
	public static class PathUtil
	{
		/// <summary>
		/// Resolve a relative path for the File System.
		/// It will replace parse the paths as it was a directory navigation, so a '..' would be the same as moving a directory up.
		/// </summary>
		/// <param name="separator">The directory separator</param>
		/// <param name="paths">
		///		The many paths to use in the resolution. If a path starts with '/', everything before all other paths before it will be ignored.
		/// </param>
		/// <returns>The resolved path</returns>
		/// <exception cref="RootPathViolationException" />
		public static string ResolveWithSeparator(string separator, params string[] paths)
		{
			var results = new System.Collections.Generic.LinkedList<string>();

			foreach(var path in paths.Where(x => x != null))
			{
				if(path.StartsWith("/") || path.StartsWith("\\"))
					results.Clear();

				foreach(var pathPart in path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries))
				{
					if(pathPart == "..")
					{
						if(results.Count == 0)
							throw new RootPathViolationException();

						results.RemoveLast();
					}
					else if(pathPart != ".")
						results.AddLast(pathPart);
				}
			}

			return separator + string.Join(separator, results);
		}

		/// <summary>
		/// Resolve a relative path for the File System.
		/// It will replace parse the paths as it was a directory navigation, so a '..' would be the same as moving a directory up.
		/// </summary>
		/// <param name="paths">The many paths to use in the resolution. If a path starts with '/', everything before all other paths before it will be ignored.</param>
		/// <returns>The resolved path</returns>
		/// <exception cref="RootPathViolationException" />
		public static string Resolve(params string[] paths)
		{
			return ResolveWithSeparator("/", paths);
		}
	}
}