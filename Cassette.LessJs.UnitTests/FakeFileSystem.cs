﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cassette.IO;
using Cassette.Utilities;

namespace Cassette.Stylesheets
{
#pragma warning disable 0659 // Don't need to use GetHashCode
    public class FakeFileSystem : IEnumerable, IDirectory
    {
        private readonly Dictionary<string, IFile> files;
        private FakeFileSystem root;

        public FakeFileSystem()
        {
            FullPath = "~";
            files = new Dictionary<string, IFile>();
        }

        private FakeFileSystem(IEnumerable<KeyValuePair<string, IFile>> files)
        {
            this.files = files.ToDictionary(f => f.Key, f => f.Value);
        }

        public FileAttributes Attributes
        {
            get { return FileAttributes.Normal; }
        }

        public bool Exists
        {
            get { return true; }
        }

        public string FullPath { get; set; }

        public override bool Equals(object obj)
        {
            return FullPath == ((FakeFileSystem) obj).FullPath;
        }

        public void Add(string filename)
        {
            filename = NormalizeSlashes(filename);
            Add(filename, "");
        }

        public void Add(string filename, byte[] bytes)
        {
            filename = NormalizeSlashes(filename);
            Func<IDirectory> directory = () => GetDirectory(filename.Substring(0, filename.LastIndexOf('/')));
            var file = new FakeFile(filename, directory)
            {
                Content = bytes
            };
            files.Add(filename, file);
        }

        public void Add(string filename, string content)
        {
            filename = NormalizeSlashes(filename);
            Add(filename, Encoding.UTF8.GetBytes(content));
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string path)
        {
            path = PathUtilities.AppRelative(path);
            return files.Any(f => f.Key.StartsWith(path) && f.Key != path);
        }

        public IEnumerable<IDirectory> GetDirectories()
        {
            var groups = (
                from pair in files
                let secondSlashIndex = pair.Key.IndexOf('/', 2)
                where secondSlashIndex > -1
                select new {pair, secondSlashIndex}
                ).GroupBy(x => x.pair.Key.Substring(0, x.secondSlashIndex));
#if NET35
            return groups.Select(g => new FakeFileSystem(g.Select(x => x.pair))
            {
                FullPath = g.Key,
                root = root ?? this
            }).Cast<IDirectory>();
#else
            return groups.Select(g => new FakeFileSystem(g.Select(x => x.pair))
            {
                FullPath = g.Key,
                root = root ?? this
            });
#endif
        }

        public IDirectory GetDirectory(string path)
        {
            path = PathUtilities.AppRelative(path);
            return new FakeFileSystem(files.Where(f => f.Key.StartsWith(path)))
            {
                FullPath = path,
                root = root ?? this
            };
        }

        public IFile GetFile(string filename)
        {
            filename = NormalizeSlashes(filename);
            if (filename.StartsWith("~"))
            {
                if (root != null)
                {
                    return root.GetFile(filename);
                }
                else
                {
                    if (files.ContainsKey(filename))
                    {
                        return files[filename];
                    }
                    else
                    {
                        return new NonExistentFile(filename);
                    }
                }
            }
            else
            {
                filename = PathUtilities.NormalizePath(FullPath + "/" + filename);
                return GetFile(filename);
            }
        }

        public IEnumerable<IFile> GetFiles(string searchPattern, SearchOption searchOption)
        {
            if (searchPattern == "*.*") return files.Values;

            var extensions = searchPattern.Split(';').Select(e => e.Substring(1)).ToArray();
            return files.Values.Where(
                file => extensions.Contains(Path.GetExtension(file.FullPath))
                );
        }

        public IDisposable WatchForChanges(Action<string> pathCreated, Action<string> pathChanged,
            Action<string> pathDeleted, Action<string, string> pathRenamed)
        {
            throw new NotImplementedException();
        }

        private static string NormalizeSlashes(string filename)
        {
            return string.Join("/", filename.Split(new[] {'/', '\\'}));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return files.GetEnumerator();
        }
    }
#pragma warning restore 0659

    public class FakeFile : IFile
    {
        private readonly Func<IDirectory> getDirectory;

        public FakeFile(string fullPath, Func<IDirectory> directory)
        {
            FullPath = fullPath;
            getDirectory = directory;
            Exists = true;
        }

        public byte[] Content { get; set; }

        public IDirectory Directory
        {
            get { return getDirectory(); }
        }

        public bool Exists { get; set; }

        public string FullPath { get; set; }

        public DateTime LastWriteTimeUtc
        {
            get { throw new NotImplementedException(); }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Stream Open(FileMode mode, FileAccess access, FileShare fileShare)
        {
            if (Content == null) return Stream.Null;
            return new MemoryStream(Content);
        }
    }
}