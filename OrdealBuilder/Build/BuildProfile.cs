using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace OrdealBuilder.Build
{
    public class BuildProfile
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Extension { get; set; }
        public string OutputDir { get; set; }
        public string Compiler { get; set; }
        public string LibsDir { get; set; }
        public List<string> Libs { get; set; }
        public List<string> CompilerDefines { get; set; }
        [JsonIgnore]
        public BuildProfileType _ProfileType;

        public BuildProfile(BuildProfileType ProfileType)
        {
            _ProfileType = ProfileType;
            Name = "App";
            Type = "Executable";
            Extension = ".c";
            Compiler = "gcc";
            LibsDir = "libs";
            OutputDir = "";
            Libs = new List<string>()
            {
                "-lSDL2",
                "-lopengl32",
                "-lglu32",
                "-lm"
            };

            CompilerDefines = new List<string>();
        }

        public BuildProfile() { }

        public void Save(string OutputDirectory, out string ProfileName)
        {
            string fileName = _ProfileType.ToString() + "_config.json";
            ProfileName = fileName;

            OutputDir = OutputDirectory;
            if (_ProfileType == BuildProfileType.Debug)
            {
                CompilerDefines.Add("DEBUG");
            }

            string jsonString = JsonSerializer.Serialize(this);
            Directory? rootDir = Project.Get().RootDirectory;
            if(rootDir != null)
            {
                string path = Path.Combine(rootDir.Path, fileName);
                System.IO.File.WriteAllText(path, jsonString);
            }
            if (_ProfileType == BuildProfileType.Debug)
            {
                CompilerDefines.Remove("DEBUG");
            }
        }
    }
}
