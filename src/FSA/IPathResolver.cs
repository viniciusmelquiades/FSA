using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSA
{
	public interface IPathResolver
	{
		string Resolve(IEnumerable<string> resolve);
	}
}
