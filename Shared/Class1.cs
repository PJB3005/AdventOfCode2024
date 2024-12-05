using System.Reflection;

namespace Shared;

public static class Input
{
    public static string Get(string name)
    {
        var executable = Assembly.GetEntryAssembly()?.Location;
        var path = Path.Combine(Path.GetDirectoryName(executable)!, "..", "..", "..", "..", "Inputs", $"{name}.txt");
        return File.ReadAllText(path);
    }
}
