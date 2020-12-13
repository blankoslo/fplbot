using System;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;


namespace FplBot.Build
{
    [CheckBuildProjectConfigurations]
    [UnsetVisualStudioEnvironmentVariables]
    class TheNukeBuild : NukeBuild
    {
        /// Support plugins are available for:
        ///   - JetBrains ReSharper        https://nuke.build/resharper
        ///   - JetBrains Rider            https://nuke.build/rider
        ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
        ///   - Microsoft VSCode           https://nuke.build/vscode

        public static int Main() => Execute<TheNukeBuild>(x => x.PackFplClient);

        [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
        readonly Configuration Configuration = Configuration.Debug;

        [Solution] readonly Solution Solution;
        [GitRepository] readonly GitRepository GitRepository;

        AbsolutePath SourceDirectory => RootDirectory / "src";
        AbsolutePath OutputDirectory => RootDirectory / "releases";

        Target Clean => _ => _
            .Executes(() =>
            {
                SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
                EnsureCleanDirectory(OutputDirectory);
            });

        Target Restore => _ => _
            .DependsOn(Clean)
            .Executes(() =>
            {
                DotNetRestore(_ => _
                    .SetProjectFile(Solution));
            });

        Target Build => _ => _
            .DependsOn(Restore)
            .Executes(() =>
            {
                DotNetBuild(_ => _
                    .SetProjectFile(Solution)
                    .SetConfiguration(Configuration)
                    .EnableNoRestore());
            });

        Target Test => _ => _
            .DependsOn(Build)
            .Executes(() =>
            {
                DotNetTest(_ => _
                    .SetProjectFile(Solution.GetProject("FplBot.Tests"))
                    .SetConfiguration(Configuration)
                    .EnableNoRestore()
                    .EnableNoBuild());
            });

        string FplClient = "Fpl.Client";
        string FplClientVersion = "0.3.4";

        Target PackFplClient => _ => _
            .DependsOn(Test)
            .Executes(() =>
            {
                DotNetPack(_ => _
                    .SetProject(Solution.GetProject(FplClient))
                    .SetConfiguration(Configuration)
                    .SetVersion(FplClientVersion)
                    .SetOutputDirectory(OutputDirectory)
                    .EnableNoRestore()
                    .EnableNoBuild());
            });

        Target PublishFplClient => _ => _
            .DependsOn(PackFplClient)
            .Executes(() =>
            {
                DotNetNuGetPush(_ => _
                    .SetTargetPath($"{OutputDirectory}/{FplClient}.{FplClientVersion}.nupkg")
                    .SetSource("https://api.nuget.org/v3/index.json")
                    .SetApiKey(Environment.GetEnvironmentVariable("NUGET_API_KEY")));
            });
    }
}
