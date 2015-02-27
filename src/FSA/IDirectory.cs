using System.Collections.Generic;
using System.Threading.Tasks;
namespace FSA
{
	public interface IDirectory
	{
		string Name
		{ get; }

		bool Exists
		{ get; }

		void Create();

		void Delete();

		void Move(string destination);

		IDirectory Parent 
		{ get; }

		IFile GetFile(string path);

		IDirectory GetDirectory(string path);

		IEnumerable<IFile> GetFiles();

		IEnumerable<IDirectory> GetDirectories();
	}
}