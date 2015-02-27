using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSA
{
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
		/// <exception cref="PathRootViolationException" />
		public static string Resolve(char separator, params string[] paths)
		{
			var results = new System.Collections.Generic.LinkedList<string>();

			foreach(var path in paths.Where(x => x != null))
			{
				if(path.StartsWith(("/")))
					results.Clear();

				foreach(var pathPart in path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries))
				{
					if(pathPart == "..")
					{
						if(results.Count <= 1)
							throw new PathRootViolationException();

						results.RemoveLast();
					}
					else if(pathPart != ".")
						results.AddLast(pathPart);
				}
			}

			return string.Join("/", results);
		}

		/// <summary>
		/// Resolve a relative path for the File System.
		/// It will replace parse the paths as it was a directory navigation, so a '..' would be the same as moving a directory up.
		/// </summary>
		/// <param name="paths">The many paths to use in the resolution. If a path starts with '/', everything before all other paths before it will be ignored.</param>
		/// <returns>The resolved path</returns>
		/// <exception cref="PathRootViolationException" />
		public static string Resolve(params string[] paths)
		{
			return Resolve('/', paths);
		}
	}
}
