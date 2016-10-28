using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GitCommands
{
    /// <summary>
    /// Class that implements the interface to the real filesystem.
    /// Implements IFilesystem, to provide code separation between business logic
    /// and the filesystem.
    /// </summary>
    public sealed class Filesystem : IFilesystem
    {
        public bool FileExists(string Filename)
        {
            return File.Exists(Filename);
        }

        public void MakeFileTemporaryWritable(string fileName, Action<string> writableAction)
        {
            //FileInfoExtensions.MakeFileTemporaryWritable(fileName, writableAction);

            var fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
            {
                //The file doesn't exist yet, no need to make it writable
                writableAction(fileName);
                return;
            }

            var oldAttributes = fileInfo.Attributes;
            fileInfo.Attributes = FileAttributes.Normal;
            writableAction(fileName);

            fileInfo.Refresh();
            if (fileInfo.Exists)
                fileInfo.Attributes = oldAttributes;
        }

        public string ReadAllText(string path, Encoding encoding)
        {
            return File.ReadAllText(path, encoding);
        }
    }
}
