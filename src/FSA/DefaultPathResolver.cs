using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSA
{
	public class DefaultPathResolver : IPathResolver
	{
		private readonly string separator;
		private readonly RootPathViolationBehavior rootPathViolationBehavior;

		public DefaultPathResolver(string separator = "/", RootPathViolationBehavior rootPathViolationBehavior = RootPathViolationBehavior.StayInRoot)
		{
			this.separator = separator;
			this.rootPathViolationBehavior = rootPathViolationBehavior;
		}

		public string Resolve(IEnumerable<string> paths)
		{
			var results = new LinkedList<string>();

			foreach(var path in paths.Where(x => x != null))
			{
				foreach(var pathPart in path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries))
				{
					if(pathPart == "..")
					{
						if(results.Count == 0)
							OnViolateRootPath();
						else
							results.RemoveLast();
					}
					else if(pathPart != ".")
						results.AddLast(pathPart);
				}
			}

			return separator + string.Join(separator, results);
		}

		private void OnViolateRootPath()
		{
			if(rootPathViolationBehavior == RootPathViolationBehavior.ThrowException)
				throw new RootPathViolationException();
		}
	}
}
