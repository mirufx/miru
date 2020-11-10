using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Miru.Core.Makers
{
    public static class NewMaker
    {
        public static void New(this Maker m, string name)
        {
            var newSolutionDir = A.Path(m.Solution.RootDir, name);
            
            ThrowIfNewDirectoryExist(newSolutionDir);
            
            Console.WriteLine();
            
            // root
            m.Directory();
            Console.WriteLine();
            
            var map = Maker.ReadEmbedded("_New.yml").FromYml<Dictionary<string, string>>();

            foreach (var (key, stub) in map)
            {
                var destination = key
                    .Replace("Skeleton", name)
                    .Replace('\\', Path.DirectorySeparatorChar);
                
                m.Template(stub, destination);
            }
            
            Console2.BreakLine();
            Console2.Line($"New solution created at:");
            Console2.BreakLine();
            Console2.GreenLine($"\t{m.Solution.RootDir}");
            Console2.BreakLine();
            Console2.GreenLine("Good luck!");
        }

        private static void ThrowIfNewDirectoryExist(MiruPath newSolutionDir)
        {
            if (Directory.Exists(newSolutionDir))
                throw new InvalidOperationException(
                    $"Can't create new Miru solution. Directory {newSolutionDir} already exist");
        }
    }
}