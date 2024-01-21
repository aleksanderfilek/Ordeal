using OrdealBuilder.Build.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrdealBuilder.Build
{
    class BuildProcess
    {
        private BuildData data;

        public BuildProcess(string OutputDir) 
        {
            ProjectPreferences? projectPreferences = Project.GetProjectPreferences();
            if(projectPreferences == null)
            {
                return;
            }
            BuildProfile profile = projectPreferences.BuildProfiles[(int)BuildProfileType.Debug];
            //profile.Save(OutputDir, out profileName);

            data = new BuildData()
            {
                OutputDir = OutputDir,
                Profile = profile
            };
        }

        public void Start()
        {
            OutputLogManager.Log(OutputLogType.Normal, "Start build");

            OutputLogManager.Log(OutputLogType.Normal, "Compiling");
            new Clean().Run(data);

            bool compileStepResult = new Compile().Run(data);
            if (!compileStepResult)
            {
                OutputLogManager.Log(OutputLogType.Error, "Compilation failed!");
                return;
            }

            OutputLogManager.Log(OutputLogType.Normal, "Copying libraries");
            bool copyLibsStepResult = new CopyLibs().Run(data);
            if (!copyLibsStepResult)
            {
                OutputLogManager.Log(OutputLogType.Error, "Copying libs failed!");
                return;
            }

            OutputLogManager.Log(OutputLogType.Normal, "Copying resources");
            bool copyResourceStepResult = new CopyResources().Run(data);
            if (!copyResourceStepResult)
            {
                OutputLogManager.Log(OutputLogType.Error, "Copying resources failed!");
                return;
            }

            OutputLogManager.Log(OutputLogType.Normal, "Build successful!");
        }
    }
}
