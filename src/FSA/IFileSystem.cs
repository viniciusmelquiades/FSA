namespace FSA
{
	public interface IFileSystem
	{
		IDirectory Root
		{ get; }

		IFile GetFile(params string[] pathParts);

		IDirectory GetDirectory(params string[] pathParts);

		IPathResolver PathResolver 
		{ get; }
	}
}