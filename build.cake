#addin Cake.Git

var target = Argument("target", "Default");
var artifactsDir = "./artifacts/";
var solutionPath = "./CodingMilitia.MoarCollections.sln";
var project = "./src/CodingMilitia.MoarCollections/CodingMilitia.MoarCollections.csproj";
var testProject = "./tests/CodingMilitia.MoarCollections.Tests/CodingMilitia.MoarCollections.Tests.csproj";
var currentBranch = GitBranchCurrent("./").FriendlyName;
var isReleaseBuild = currentBranch == "master";


Task("Clean")
    .Does(() => {
        if (DirectoryExists(artifactsDir))
        {
            DeleteDirectory(artifactsDir, recursive:true);
        }
        CreateDirectory(artifactsDir);
    });

Task("Restore")
    .Does(() => {
        DotNetCoreRestore(solutionPath);
    });

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() => {
        DotNetCoreBuild(solutionPath,
                new DotNetCoreBuildSettings 
                {
                    Configuration = isReleaseBuild ? "Release" : "Debug"
                }
        );
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetCoreTest(testProject);
    });

Task("Package")
    .IsDependentOn("Test")
    .Does(() => {
        PackageProject("CodingMilitia.MoarCollections", project, artifactsDir);
    });

if(isReleaseBuild)
{
    Task("Default")
        .IsDependentOn("Package");
}
else
{
    Task("Default")
        .IsDependentOn("Test");
}

RunTarget(target);

private void PackageProject(string projectName, string projectPath, string outputDirectory)
{
    var settings = new DotNetCorePackSettings
        {
            OutputDirectory = outputDirectory,
            NoBuild = true
        };

    DotNetCorePack(projectPath, settings);
}    