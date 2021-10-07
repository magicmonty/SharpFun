using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.ChangeLog.ChangelogTasks;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Test);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    AbsolutePath TestsDirectory => RootDirectory / "Test";
    AbsolutePath DeployDirectory => RootDirectory / "deploy";

    AbsolutePath ChangeLogFile = RootDirectory / "CHANGELOG.md";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            RootDirectory
                .GlobDirectories("**/bin", "**/obj", "**/TestResults")
                .ForEach(DeleteDirectory);

            EnsureCleanDirectory(DeployDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
            DotNetRestore(s => s
                .SetProjectFile(Solution)));

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(TestsDirectory / "Test.csproj")
            .SetConfiguration(Configuration)
            .SetDataCollector("XPlat Code Coverage")
            .SetSettingsFile(TestsDirectory / "coverlet.runsettings")
            .SetLoggers("trx")));

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var changeLog = ReadChangelog(ChangeLogFile);
            var version = changeLog.ReleaseNotes.Last().Version;
            var releaseNotes =GetNuGetReleaseNotes(ChangeLogFile);

            return DotNetPack(s => s
                .SetProject(RootDirectory / "Functional")
                .SetConfiguration(Configuration)
                .SetVersionPrefix(version.ToString())
                .SetPackageReleaseNotes(releaseNotes)
                .SetOutputDirectory(DeployDirectory));
        });
}
