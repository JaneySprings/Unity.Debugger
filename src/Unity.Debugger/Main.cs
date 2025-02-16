namespace Unity.Debugger;

public class Program {
    private static void Main(string[] args) {
        var debugSession = new DebugSession(Console.OpenStandardInput(), Console.OpenStandardOutput());
        debugSession.Start();
    }
}
