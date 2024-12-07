namespace Shared;

public static class Input
{
    public static string Get(string name)
    {
        var path = Path.Combine(GetSolutionDir(), "Inputs", $"{name}.txt");
        return File.ReadAllText(path);
    }

    private static string GetSolutionDir()
    {
        var dir = Environment.CurrentDirectory;
        while (true)
        {
            if (File.Exists(Path.Combine(dir, "AdventOfCode2024.sln")))
                return dir;

            var basePath = Path.GetDirectoryName(dir);
            dir = basePath ?? throw new InvalidCastException("Unable to find solution directory!");
        }
    }
}
