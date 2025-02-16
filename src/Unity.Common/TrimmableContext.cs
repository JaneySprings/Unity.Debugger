using System.Text.Json.Serialization;

namespace Unity.Common;

[JsonSerializable(typeof(EditorInstance))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(int[]))]
public partial class TrimmableContext : JsonSerializerContext {}