using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace images_mover
{
    class Program
    {
        // args[0]: path to the uospc repo
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(args[0], "*.*", SearchOption.AllDirectories)
                                .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg"));
            Directory.CreateDirectory("/tmp/static/");
            foreach (string file in files)
            {
                Console.WriteLine("Moving " + file + "...");
                string path = Path.Combine("/tmp/static/", Path.GetFileName(file));
                File.Copy(file, path, true);
            }
        }
    }
}
