using System.Diagnostics;
using Shared;

var sw = Stopwatch.StartNew();
var input = Input.Get("day08");
var grid = ParseGrid(input);

foreach (var (freq, list) in grid.Antennae)
{
    Console.WriteLine($"{freq}: {string.Join(", ", list.Select(x => x.ToString()))}");
}

var antinodes = grid.Antennae.Select(kv =>
{
    var antinodes = CalculateAntinodes(kv.Value).Distinct();

    return (kv.Key, antinodes);
}).ToDictionary();

var totalCount = antinodes.SelectMany(kv => kv.Value).Distinct().Where(grid.IsInBounds).Count();

Console.WriteLine($"{totalCount}");
Console.WriteLine(sw.Elapsed);

return;

static IEnumerable<Vector2i> CalculateAntinodes(List<Vector2i> antennae)
{
    foreach (var antenna in antennae)
    {
        foreach (var otherAntenna in antennae)
        {
            if (antenna == otherAntenna)
                continue;

            var diff = antenna - otherAntenna;

            yield return antenna + diff;
            yield return otherAntenna - diff;
        }
    }
}

static Grid ParseGrid(string input)
{
    var grid = new Grid();

    var reader = new StringReader(input);
    var width = 0;
    var y = 0;
    while (reader.ReadLine()?.Trim() is { Length: > 0 } line)
    {
        width = line.Length;
        for (var x = 0; x < line.Length; x++)
        {
            var chr = line[x];
            if (chr == '.')
                continue;

            var freq = new Frequency(chr);
            var vec = new Vector2i(x, y);
            grid.Antennae.GetValueOrNew(freq).Add(vec);
        }

        y += 1;
    }

    grid.Width = width;
    grid.Height = y;

    return grid;
}

internal sealed class Grid
{
    public readonly Dictionary<Frequency, List<Vector2i>> Antennae = new();
    public int Width;
    public int Height;

    public bool IsInBounds(Vector2i point)
    {
        return point.X >= 0 && point.X < Width && point.Y >= 0 && point.Y < Height;
    }
}

internal readonly record struct Frequency(byte Value)
{
    public Frequency(char chr) : this((byte) chr)
    {
        Debug.Assert(chr is ((>= '0' and <= '9') or (>= 'A' and <= 'Z') or (>= 'a' and <= 'z')));
    }

    public override string ToString()
    {
        return ((char)Value).ToString();
    }
}
