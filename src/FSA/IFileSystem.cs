namespace FSA
{
	public interface IFileSystem
	{
		IDirectory Root
		{ get; }

		IFile GetFile(string path);

		IDirectory GetDirectory(string path);
	}
}