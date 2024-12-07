using System.Diagnostics;
using System.Text;
using Shared;

var sw = Stopwatch.StartNew();

var input = Input.Get("day06");
var grid = ParseGrid(input, out var guardPos);
var guardDir = GuardDir.Up;

Console.WriteLine($"Initial guard pos is {guardPos}");
Console.WriteLine($"Grid size: {grid.GetLength(0)}, {grid.GetLength(1)}");

var countParadoxes = 0;

// Walk the guard.
Walk(grid, guardPos, guardDir, (curPos, moveDir) =>
{
    var fluxPos = Step(curPos, moveDir);
    if ((Idx(grid, fluxPos) & Cell.AllDirs) != 0)
    {
        // Already been through there, can't be it.
        return;
    }

    var newGrid = CloneGrid(grid);

    ref var flux = ref Idx(newGrid, fluxPos);
    flux |= Cell.Wall;

    if (Walk(newGrid, curPos, Turn(moveDir)) == WalkResult.Looped)
    {
        countParadoxes += 1;
        Idx(grid, fluxPos) |= Cell.Candidate;
    }
});

Console.WriteLine(countParadoxes);
Console.WriteLine(sw.Elapsed);

var sb = new StringBuilder();

for (var y = 0; y < grid.GetLength(1); y++)
{
    for (var x = 0; x < grid.GetLength(0); x++)
    {
        var cell = grid[x, y];
        if ((cell & Cell.Wall) != 0)
        {
            sb.Append('#');
        }
        else if ((cell & Cell.Candidate) != 0)
        {
            sb.Append('O');
        }
        else
        {
            var ns = (cell & Cell.NorthBound) != 0 || (cell & Cell.SouthBound) != 0;
            var ew = (cell & Cell.EastBound) != 0 || (cell & Cell.WestBound) != 0;
            if (ns && ew)
            {
                sb.Append('+');
            }
            else if (ns)
            {
                sb.Append('|');
            }
            else if (ew)
            {
                sb.Append('-');
            }
            else
            {
                sb.Append('.');
            }
        }
    }

    sb.AppendLine();
}



Console.WriteLine(sb.ToString());


return;

static Cell[,] CloneGrid(Cell[,] grid)
{
    return (Cell[,])grid.Clone();
}

static WalkResult Walk(
    Cell[,] grid,
    Vector2i startPos,
    GuardDir startDir,
    Action<Vector2i, GuardDir>? evaluateParadox = null)
{
    var guardPos = startPos;
    var guardDir = startDir;
    while (true)
    {
        ref var cell = ref Idx(grid, guardPos);
        var bit = BoundBit(guardDir);
        if ((cell & bit) != 0)
            return WalkResult.Looped;

        cell |= bit;

        var nextPos = Step(guardPos, guardDir);

        if (!IsInBounds(grid, nextPos))
            return WalkResult.Left;

        if ((Idx(grid, nextPos) & Cell.Wall) != 0)
        {
            // Turn right
            guardDir = Turn(guardDir);
        }
        else
        {
            evaluateParadox?.Invoke(guardPos, guardDir);
            guardPos = nextPos;
        }
    }
}

static Cell BoundBit(GuardDir dir)
{
    return dir switch
    {
        GuardDir.Up => Cell.NorthBound,
        GuardDir.Right => Cell.EastBound,
        GuardDir.Down => Cell.SouthBound,
        GuardDir.Left => Cell.WestBound,
        _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
    };
}

static Vector2i Step(Vector2i pos, GuardDir dir)
{
    return dir switch
    {
        GuardDir.Up => pos + new Vector2i(0, -1),
        GuardDir.Down => pos + new Vector2i(0, 1),
        GuardDir.Left => pos + new Vector2i(-1, 0),
        GuardDir.Right => pos + new Vector2i(1, 0),
        _ => throw new ArgumentOutOfRangeException()
    };
}

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
                    cell |= Cell.Wall;
                    break;
                case '^':
                    guardPos = new Vector2i(x, y);
                    break;
            }
        }
    }

    return grid;
}

static GuardDir Turn(GuardDir guardDir1)
{
    return (GuardDir)((int)(guardDir1 + 1) % 4);
}

[Flags]
internal enum Cell : byte
{
    None = 0,
    Wall = 1 << 0,
    NorthBound = 1 << 1,
    EastBound = 1 << 2,
    SouthBound = 1 << 3,
    WestBound = 1 << 4,
    AllDirs = NorthBound | EastBound | SouthBound | WestBound,

    Candidate = 1 << 5,
}

enum GuardDir
{
    Up,
    Right,
    Down,
    Left
}

enum WalkResult
{
    Left,
    Looped,
}

internal struct Vector2i(int x, int y) : IEquatable<Vector2i>
{
    public int X = x;
    public int Y = y;

    public static Vector2i operator +(Vector2i left, Vector2i right)
    {
        return new Vector2i(left.X + right.X, left.Y + right.Y);
    }

    public bool Equals(Vector2i other)
    {
        return other.X == X && other.Y == Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"{X}, {Y}";
    }
}