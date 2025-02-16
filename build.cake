using System.Runtime.InteropServices;
using _Path = System.IO.Path;

public string RootDirectory => MakeAbsolute(Directory("./")).ToString();
public string ArtifactsDirectory => _Path.Combine(RootDirectory, "artifacts");

var target = Argument("target", "debugger");
var version = Argument("release-version", "1.0.0");
var configuration = Argument("configuration", "debug");
var runtime = Argument("arch", RuntimeInformation.RuntimeIdentifier);


Task("clean").Does(() => {
	EnsureDirectoryExists(ArtifactsDirectory);
	CleanDirectories(_Path.Combine(RootDirectory, "src", "**", "bin"));
	CleanDirectories(_Path.Combine(RootDirectory, "src", "**", "obj"));
});

Task("debugger")
	.Does(() => DotNetPublish(_Path.Combine(RootDirectory, "src", "Unity.Debugger", "Unity.Debugger.csproj"), new DotNetPublishSettings {
		MSBuildSettings = new DotNetMSBuildSettings { 
			ArgumentCustomization = args => args.Append("/p:NuGetVersionRoslyn=4.5.0"),
			AssemblyVersion = version
		},
		Configuration = configuration,
		Runtime = runtime,
	}));


RunTarget(target);