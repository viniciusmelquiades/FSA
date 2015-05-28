using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSA
{
	public enum RootPathViolationBehavior
	{
		/// <summary>
		/// When trying to navigate above the root, nothing happens.
		/// </summary>
		StayInRoot,
		/// <summary>
		/// When trying to navigate above the root, an <see cref="RootPathViolationException"/> will be thrown.
		/// </summary>
		ThrowException
	}
}
