using Unity.Debugger.Extensions;
using Newtonsoft.Json.Linq;
using Mono.Debugging.Client;
using Unity.Common.Extensions;
using System.Text.Json;
using Unity.Common;

namespace Unity.Debugger;

public class LaunchConfiguration {
    public string ProjectDirectory { get; init; }
    public int DebugPort { get; init; }
    public string? TransportId { get; init; }
    public Dictionary<string, string> EnvironmentVariables { get; init; }
    public DebuggerSessionOptions DebuggerSessionOptions { get; init; }

    private bool SkipDebug { get; init; }

    public LaunchConfiguration(Dictionary<string, JToken> configurationProperties) {
        SkipDebug = configurationProperties.TryGetValue("skipDebug").ToValue<bool>();
        DebugPort = configurationProperties.TryGetValue("debuggingPort").ToValue<int>();
        TransportId = configurationProperties.TryGetValue("transportId").ToClass<string>();
        DebuggerSessionOptions = configurationProperties.TryGetValue("debuggerOptions")?.ToClass<DebuggerSessionOptions>() 
            ?? ServerExtensions.DefaultDebuggerOptions;
        EnvironmentVariables = configurationProperties.TryGetValue("env")?.ToClass<Dictionary<string, string>>()
            ?? new Dictionary<string, string>();

        var projectDirectory = configurationProperties.TryGetValue("project").ToClass<string>()?.ToPlatformPath().TrimPathEnd();
        if (!Directory.Exists(projectDirectory))
            throw ServerExtensions.GetProtocolException($"Incorrect project directory: '{projectDirectory}'");

        ProjectDirectory = projectDirectory;
    }

    public string GetAssembliesPath() {
        return Path.Combine(ProjectDirectory, "Library", "ScriptAssemblies");
    }
    public string GetProjectName() {
        return Path.GetFileName(ProjectDirectory);
    }
    public BaseLaunchAgent GetLaunchAgent() {
        return new DebugLaunchAgent(this); //NoDebug?
    }
    public EditorInstance GetEditorInstance() {
        var editorInfo = Path.Combine(ProjectDirectory, "Library", "EditorInstance.json");
        if (!File.Exists(editorInfo))
            throw ServerExtensions.GetProtocolException($"EditorInstance.json not found: '{editorInfo}'");

        var editorInstance = JsonSerializer.Deserialize<EditorInstance>(File.ReadAllText(editorInfo), TrimmableContext.Default.EditorInstance);
        if (editorInstance == null)
            throw ServerExtensions.GetProtocolException($"Failed to deserialize EditorInstance.json: '{editorInfo}'");

        return editorInstance;
    }
}