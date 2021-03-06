﻿using System;
using System.IO;

namespace Cassette.Stylesheets
{
    public class TempDirectory : IDisposable
    {
        private readonly string path;

        public TempDirectory()
        {
            path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);
        }

        public void Dispose()
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        #region Operators

        public static implicit operator string(TempDirectory directory)
        {
            return directory.path;
        }

        #endregion
    }
}