using System.Diagnostics;
using System.Text;
using Shared;

var input = Input.Get("day06");
var grid = ParseGrid(input, out var guardPos);
var guardDir = GuardDir.Up;

Console.WriteLine($"Initial guard pos is {guardPos}");
Console.WriteLine($"Grid size: {grid.GetLength(0)}, {grid.GetLength(1)}");

// Walk the guard.
while (true)
{
    if (!IsInBounds(grid, guardPos))
        break;

    Idx(grid, guardPos).Visited = true;

    var nextPos = guardDir switch
    {
        GuardDir.Up => guardPos + new Vector2i(0, -1),
        GuardDir.Down => guardPos + new Vector2i(0, 1),
        GuardDir.Left => guardPos + new Vector2i(-1, 0),
        GuardDir.Right => guardPos + new Vector2i(1, 0),
        _ => throw new ArgumentOutOfRangeException()
    };

    if (IsInBounds(grid, nextPos) && Idx(grid, nextPos).Wall)
    {
        // Turn right
        guardDir = (GuardDir)((int)(guardDir + 1) % 4);
    }
    else
    {
        guardPos = nextPos;
    }
}

Debug.Assert(!Enumerate(grid).Any(x => x.Visited && x.Wall));

Console.WriteLine(Enumerate(grid).Count(x => x.Visited));

var sb = new StringBuilder();

for (var y = 0; y < grid.GetLength(1); y++)
{
    for (var x = 0; x < grid.GetLength(0); x++)
    {
        var cell = grid[x, y];
        if (cell.Visited)
        {
            sb.Append('X');
        }
        else if (cell.Wall)
        {
            sb.Append('#');
        }
        else
        {
            sb.Append('.');
        }
    }

    sb.AppendLine();
}

Console.WriteLine(sb.ToString());



return;

static ref T Idx<T>(T[,] grid, Vector2i input)
{
    return ref grid[input.X, input.Y];
}

static bool IsInBounds<T>(T[,] grid, Vector2i input)
{
    return input.X >= 0
           && input.X < grid.GetLength(0)
           && input.Y >= 0
           && input.Y < grid.GetLength(1);
}

static Cell[,] ParseGrid(string text, out Vector2i guardPos)
{
    guardPos = default;

    var lines = new List<string>();
    var reader = new StringReader(text);
    while (reader.ReadLine() is { } line)
    {
        lines.Add(line.Trim());
    }

    var sizeX = lines[0].Length;
    var sizeY = lines.Count;
    var grid = new Cell[sizeX, sizeY];
    for (var y = 0; y < sizeY; y++)
    {
        var line = lines[y];
        Console.WriteLine(line);
        for (var x = 0; x < sizeX; x++)
        {
            var chr = line[x];
            ref var cell = ref grid[x, y];
            switch (chr)
            {
                case '#':
                    cell.Wall = true;
                    break;
                case '^':
                    guardPos = new Vector2i(x, y);
                    break;
            }
        }
    }

    return grid;
}

static IEnumerable<T> Enumerate<T>(T[,] grid)
{
    for (var x = 0; x < grid.GetLength(0); x++)
    {
        for (var y = 0; y < grid.GetLength(1); y++)
        {
            yield return grid[x, y];
        }
    }
}

internal struct Cell
{
    public bool Wall;
    public bool Visited;
}

enum GuardDir
{
    Up,
    Right,
    Down,
    Left
}