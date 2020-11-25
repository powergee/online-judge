using System;
using System.IO;
using System.Collections.Generic;

namespace conf_tc
{
    class TestCase
    {
        public string Input { get; }
        public string Output { get; }

        public TestCase(string inputPath, string outputPath)
        {
            Input = File.ReadAllText(inputPath);
            Output = File.ReadAllText(outputPath);
        }
    }

    class TestCaseSet
    {
        public string ID { get; }
        public List<TestCase> Cases { get; } = new List<TestCase>();

        public TestCaseSet(string tcDir)
        {
            string parentPath = Path.GetDirectoryName(tcDir);
            string parentName = Path.GetFileName(parentPath);
            Console.WriteLine($"Found a problem directory : {parentName}");
            parentName = parentName.Replace("_", "");
            parentName = parentName.Replace("-", "");
            ID = parentName;

            string[] files = Directory.GetFiles(tcDir);
            Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>();
            foreach (string file in files)
            {
                if (file.Contains('.'))
                {
                    string name = file.Substring(0, file.LastIndexOf('.'));

                    if (!groups.ContainsKey(name))
                        groups.Add(name, new List<string>());
                    groups[name].Add(file);
                }
                else
                    Console.WriteLine($"[WARN] The name of file doesn't contain a period. It will be ignored. ({file})");
            }

            foreach (var pair in groups)
            {
                string inputPath = null, outputPath = null;
                foreach (string file in pair.Value)
                {
                    string ext = file.Substring(file.LastIndexOf('.')+1);
                    if (ext == "in")
                        inputPath = file;
                    else if (ext == "out")
                        outputPath = file;
                    else
                        Console.WriteLine($"[WARN] There's an invalid extension. Only in and out will be allowed. It will be ignored. ({file})");
                }
                if (inputPath != null && outputPath != null)
                    Cases.Add(new TestCase(inputPath, outputPath));
                else
                    Console.WriteLine($"Can't find a right I/O pair. (input:{(inputPath == null ? "Null" : inputPath)}, output:{(outputPath == null ? "Null" : outputPath)})");
            }
        }

        public void WriteConfigFiles(string root)
        {
            string dir = Path.Combine(root, ID);
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
            
            int index = 1;
            string ymlPath = Path.Combine(dir, "init.yml");
            using (TextWriter yWriter = File.CreateText(ymlPath))
            {
                yWriter.WriteLine("test_cases:");
                foreach (TestCase pair in Cases)
                {
                    string inputPath = Path.Combine(dir, $"{index}.in");
                    string outputPath = Path.Combine(dir, $"{index}.out");
                    yWriter.WriteLine($" - {{in: {index}.in, out: {index}.out, points: {(index == Cases.Count ? 100 : 0)}}}");
                    ++index;
                    using (TextWriter iWriter = File.CreateText(inputPath))
                    using (TextWriter oWriter = File.CreateText(outputPath))
                    {
                        iWriter.WriteLine(pair.Input);
                        oWriter.WriteLine(pair.Output);
                    }
                }
            }
        }
    }

    class Program
    {
        // args[0]: path to the uospc repo
        // args[1]: path to the "problems" directory
        static void Main(string[] args)
        {
            string[] dirs = Directory.GetDirectories(args[0], "testcases", SearchOption.AllDirectories);

            List<TestCaseSet> tcsets = new List<TestCaseSet>();
            Console.WriteLine("Finding all testcases...");
            foreach (string dir in dirs)
                tcsets.Add(new TestCaseSet(dir));
            
            Console.WriteLine($"Found {dirs.Length} problems. Writing configs...");
            foreach (TestCaseSet set in tcsets)
                set.WriteConfigFiles(args[1]);
            Console.WriteLine("Finished.");
        }
    }
}
