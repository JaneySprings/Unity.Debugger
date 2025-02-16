namespace Unity.Common.Extensions;

public static class StringExtensions {
    public static string TrimStart(this string target, string trimString) {
        if (string.IsNullOrEmpty(trimString))
            return target;

        if (target.StartsWith(trimString, StringComparison.OrdinalIgnoreCase))
            return target.Substring(trimString.Length);

        return target;
    }
    public static string TrimEnd(this string target, string trimString) {
        if (string.IsNullOrEmpty(trimString))
            return target;

        if (target.EndsWith(trimString, StringComparison.OrdinalIgnoreCase))
            return target.Substring(0, target.Length - trimString.Length);

        return target;
    }
    public static bool ContainsInsensitive(this string? source, string value) {
        return source?.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
    }
    public static bool EqualsInsensitive(this string? source, string value) {
        return source?.Equals(value, StringComparison.OrdinalIgnoreCase) == true;
    }

    public static string ToPlatformPath(this string path) {
        return path
            .Replace('\\', System.IO.Path.DirectorySeparatorChar)
            .Replace('/', System.IO.Path.DirectorySeparatorChar)
            .Replace("\\\\", $"{System.IO.Path.DirectorySeparatorChar}")
            .Replace("//", $"{System.IO.Path.DirectorySeparatorChar}");
    }
    public static string TrimPathEnd(this string path) {
        return path.TrimEnd(System.IO.Path.DirectorySeparatorChar);
    }
}