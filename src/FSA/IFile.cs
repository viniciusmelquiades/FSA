using System.IO;

namespace FSA
{
	public interface IFile
	{
		string Name
		{ get; }

		bool Exists
		{ get; }

		IDirectory Directory
		{ get; }

		IFile Move(string path, bool replace = false);

		IFile Copy(string path, bool replace = false);

		void Delete();

		Stream OpenRead();

		Stream OpenWrite(bool append = true);
	}
}