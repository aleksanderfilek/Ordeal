using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdealBuilder;

namespace OrdealBuilder
{
    public class DirectoryChangedArgs : EventArgs
    {
        private readonly Directory _directory;

        public DirectoryChangedArgs(Directory directory)
        {
            _directory = directory;
        }

        public Directory Directory
        {
            get { return _directory; }
        }
    }

    public class Project
    {
        public static readonly string ProjectExtension = ".gamepro";

        private static Project _instance;
        public static Project? Get() { return _instance; }

        ProjectPreferences? projectPreferences = null;
        public static ProjectPreferences? GetProjectPreferences() { return _instance.projectPreferences; }

        private Directory? rootDirectory;
        public Directory? RootDirectory { get { return rootDirectory; } }
        public event EventHandler<DirectoryChangedArgs>? OnDirectoryChanged;

        public string ProjectFilePath { get; private set; }
        public string ProjectPath { get; private set; }
        public string ProjectResourcePath { get; private set; }
        public string ProjectSourcePath { get; private set; }

        private OutputLogManager outputLogManager;

        public Project()
        {
            _instance = this;
        }

        public void NewProject(string projectFilePath)
        {
            if (string.IsNullOrEmpty(projectFilePath))
            {
                rootDirectory = null;
                return;
            }

            ProjectFilePath = projectFilePath;
            ProjectPath = Path.GetDirectoryName(projectFilePath);
            ProjectResourcePath = Path.Combine(ProjectPath, "resources");
            ProjectSourcePath = Path.Combine(ProjectPath, "source");

            projectPreferences = new ProjectPreferences(Path.GetFileNameWithoutExtension(projectFilePath));
            projectPreferences.Save(projectFilePath);

            OpenProject(projectFilePath);
        }

        public void OpenProject(string? projectFilePath)
        {
            if (string.IsNullOrEmpty(projectFilePath))
            {
                rootDirectory = null;
                projectPreferences = null;
                return;
            }

            ProjectFilePath = projectFilePath;
            ProjectPath = Path.GetDirectoryName(projectFilePath);
            ProjectResourcePath = Path.Combine(ProjectPath, "resources");
            ProjectSourcePath = Path.Combine(ProjectPath, "source");

            outputLogManager = new OutputLogManager();

            projectPreferences = ProjectPreferences.Load(projectFilePath);

            rootDirectory = new Directory(ProjectResourcePath);
            OnDirectoryChanged?.Invoke(this, new DirectoryChangedArgs(rootDirectory));

            OutputLogManager.Log(OutputLogType.Normal ,"Project opened");
        }

        public void SaveAll()
        {
            if(rootDirectory != null)
            {
                rootDirectory.Save();
            }
        }

        public bool IsSomethingToSave()
        {
            if (rootDirectory == null)
                return false;
            return rootDirectory.IsSomethingToSave();
        }

        public void SaveProjectPreferences()
        {
            if (projectPreferences == null)
            {
                return;
            }
            projectPreferences.Save(ProjectPath);
        }

        public void UpdateProjectFilesExtension(string NewExtension)
        {
            string OldExtension = projectPreferences.ResourceExtension;
            projectPreferences.ResourceExtension = NewExtension;
            if (rootDirectory != null)
            {
                rootDirectory.UpdateProjectFilesExtension(OldExtension, NewExtension);
            }
        }
    }
}
