using System.IO;

namespace FSA.PhysicalFileSystem
{
	public class PhysicalFile : BaseFile
	{
		public readonly PhysicalFileSystem _fs;

		protected internal PhysicalFile(PhysicalFileSystem fs, string path)
			: base(fs, path)
		{
			_fs = fs;

			FullPath = fs.GetFullPath(path);
		}

		public string FullPath
		{ get; private set; }

		public override bool Exists
		{
			get { return File.Exists(FullPath); }
		}

		public override Stream OpenRead()
		{
			return File.OpenRead(FullPath);
		}

		protected override IDirectory GetDirectory()
		{
			return new PhysicalDirectory(_fs, System.IO.Path.GetDirectoryName(Path));
		}

		protected override void MoveFile(IFile newFile)
		{
			var physicalDestinationPath = _fs.GetFullPath(newFile.Path);

			File.Move(FullPath, physicalDestinationPath);
		}

		protected override void CopyFile(IFile newFile)
		{
			var physicalDestinationPath = _fs.GetFullPath(newFile.Path);

			File.Copy(FullPath, physicalDestinationPath);
		}

		protected override void DeleteFile()
		{
			File.Delete(FullPath);
		}

		protected override Stream GetWriteStream()
		{
			return File.OpenWrite(FullPath);
		}
	}
}