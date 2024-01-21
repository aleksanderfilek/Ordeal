using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using OrdealBuilder.Build;
using System.Text.Json.Serialization;

namespace OrdealBuilder
{
    public class ProjectPreferences
    {
        public string ResourceExtension { get; set; }
        [JsonIgnore]
        private string _ApplicationName;
        public string ApplicationName
        {
            get => _ApplicationName;
            set
            {
                _ApplicationName = value;
                if (BuildProfiles != null)
                {
                    foreach (BuildProfile profile in BuildProfiles)
                    {
                        profile.Name = _ApplicationName;
                    }
                }
            }
        }
        public string BuilderPath { get; set; }
        [JsonIgnore]
        private string _Compiler;
        public string Compiler
        {
            get => _Compiler;
            set
            {
                _Compiler = value;
                if (BuildProfiles != null)
                {
                    foreach (BuildProfile profile in BuildProfiles)
                    {
                        profile.Compiler = _Compiler;
                    }
                }
            }
        }
        [JsonIgnore]
        private string _SourceFileExtension;
        public string SourceFileExtension
        {
            get => _SourceFileExtension;
            set
            {
                _SourceFileExtension = value;
                if (BuildProfiles != null)
                {
                    foreach (BuildProfile profile in BuildProfiles)
                    {
                        profile.Extension = _SourceFileExtension;
                    }
                }
            }
        }

        public List<BuildProfile> BuildProfiles { get; set; }

        public ProjectPreferences() { }
        public ProjectPreferences(string Name) 
        {
            ResourceExtension = ".res";
            ApplicationName = Name;
            BuilderPath = "";
            BuildProfiles = new List<BuildProfile>(2)
            {
                new BuildProfile(BuildProfileType.Release),
                new BuildProfile(BuildProfileType.Debug)
            };
            Compiler = "gcc";
            SourceFileExtension = ".c";
        }

        public void Save(string ProjectFilePath)
        {
            string jsonString = JsonSerializer.Serialize(this);
            System.IO.File.WriteAllText(ProjectFilePath, jsonString);
        }

        public static ProjectPreferences? Load(string ProjectFilePath)
        {
            string jsonString = System.IO.File.ReadAllText(ProjectFilePath);
            return JsonSerializer.Deserialize<ProjectPreferences>(jsonString);
        }
    }
}
