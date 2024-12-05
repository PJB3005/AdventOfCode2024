using Shared;

var input = ToGrid(Input.Get("day04"));
var size = (int) Math.Sqrt(input.Length);

var totalCount = 0;

for (var x = 1; x < size - 1; x++)
{
    for (var y = 1; y < size - 1; y++)
    {
        if (Idx(x, y) != 'A')
            continue;

        if (HasXmas(x, y))
        {
            Console.WriteLine($"X-MAS at {x},{y}");
            totalCount += 1;
        }
    }
}

Console.WriteLine($"Result: {totalCount}");

return;

bool HasXmas(int x, int y)
{
    if (Diag(x, y, 1) is not ("MAS" or "SAM"))
        return false;

    if (Diag(x, y, -1) is not ("MAS" or "SAM"))
        return false;

    return true;
}

string Diag(int x, int y, int pitch)
{
    return new string([Idx(x - pitch, y - 1), Idx(x, y), Idx(x + pitch, y + 1)]);
}

char Idx(int x, int y) => input[y * size + x];

char[] ToGrid(string text)
{
    return text.ReplaceLineEndings("").ToCharArray();
}