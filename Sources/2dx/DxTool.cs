using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace VoxCharger
{
    public static class DxTool
    {
        private const string ConverterFileName = "2dxwavconvert.exe";
        private const string BuilderFileName   = "2dxbuild.exe";

        public static string ConvertToWave(string inputFileName, bool preview = false)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);

            string output = Path.Combine(tempDir, $"{Directory.GetFiles(tempDir).Length}.wav");
            Execute(
                ConverterFileName,
                $"\"{inputFileName}\" \"{output}\"" + (preview ? " preview" : string.Empty)
            );

            return tempDir;
        }

        public static void Build(string inputDir, string outputFileName)
        {
            Execute(
                BuilderFileName,
                $"\"{outputFileName}\"",
                inputDir
            );
        }

        private static void Execute(string fileName, string args, string workingDir = null)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException($"{fileName} not found", fileName);

            workingDir = workingDir ?? Environment.CurrentDirectory;
            var info = new ProcessStartInfo()
            {
                FileName               = fileName,
                Arguments              = args,
                WorkingDirectory       = workingDir,
                CreateNoWindow         = true,
                UseShellExecute        = false,
                RedirectStandardOutput = true
            };

            using (var process = Process.Start(info))
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                    throw new ApplicationException($"{fileName} execution failed:\n{process.StandardOutput.ReadToEnd()}");
            }
        }

    }
}
