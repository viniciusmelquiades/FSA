using System.Collections.Generic;

namespace FSA
{
	public interface IDirectory
	{
		string Path { get; }

		string Name { get; }

		bool Exists { get; }

		IDirectory Parent { get; }

		void Create();

		void Delete();

		IDirectory Move(string destination);

		IFile GetFile(string path);

		IDirectory GetDirectory(string path);

		IEnumerable<IFile> GetFiles();

		IEnumerable<IDirectory> GetDirectories();
	}
}