namespace Gu.Wpf.ModernUI.UITests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    using NUnit.Framework;

    public static class Info
    {
        internal static ProcessStartInfo CreateStartInfo(string appName, string windowName)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = GetExeFileName(appName),
                Arguments = windowName,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            return processStartInfo;
        }

        internal static string TestAssemblyFullFileName()
        {
            return new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
        }

        internal static string TestAssemblyDirectory() => Path.GetDirectoryName(TestAssemblyFullFileName());

        internal static string ArtifactsDirectory()
        {
            // ReSharper disable PossibleNullReferenceException
            var root = new DirectoryInfo(TestAssemblyFullFileName()).Parent.Parent.Parent.Parent.FullName;
            // ReSharper restore PossibleNullReferenceException
            var artifacts = Path.Combine(root, "artifacts");
            Directory.CreateDirectory(artifacts);
            return artifacts;
        }

        private static string GetExeFileName(string appName)
        {
            var fileName = Path.Combine(TestAssemblyDirectory(), appName)
                               .ChangeExtension(".exe");
            Assert.IsTrue(File.Exists(fileName), "Could not find exe");
            return fileName;
        }

        private static string ChangeExtension(this string fullFileName, string extension)
        {
            return Path.ChangeExtension(fullFileName, extension);
        }
    }
}
