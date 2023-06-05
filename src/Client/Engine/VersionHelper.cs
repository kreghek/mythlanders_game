using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Rpg.Client.Engine
{
    internal static class VersionHelper
    {
        public static bool TryReadVersion([NotNullWhen(true)] out string? version)
        {
            var binPath = AppContext.BaseDirectory;

            if (string.IsNullOrWhiteSpace(binPath))
            {
                throw new InvalidOperationException("Path to bin directory is null.");
            }

            var versionFile = Path.Combine(binPath, "version.txt");

            if (File.Exists(versionFile))
            {
                version = File.ReadAllText(versionFile);
                return true;
            }

            version = null;
            return true;
        }
    }
}