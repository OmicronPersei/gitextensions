using System;
using System.Text;

namespace GitCommands
{
    /// <summary>
    /// Provides an interface separating code calls and the filesystem itself.
    /// 
    /// This is to be used as a separation between the code and direct calls to the filesystem,
    /// to facilitate unit testing.
    /// </summary>
    public interface IFilesystem
    {
        bool FileExists(string Filename);

        /// <summary>
        /// Performs a write operation on a file that may not exist.
        /// 
        /// For files that already exist, temporarily modify permissions, write
        /// to the file, then restore permissions when completed.
        /// </summary>
        void MakeFileTemporaryWritable(string fileName, Action<string> writableAction);

        string ReadAllText(string path, Encoding encoding);
    }
}
