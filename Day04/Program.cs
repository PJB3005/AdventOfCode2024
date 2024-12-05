using System.Text.RegularExpressions;
using Shared;

var xmasRegex = new Regex("XMAS");
var inverseXmasRegex = new Regex("SAMX");
var input = ToGrid(Input.Get("day04"));
var size = 140;

var totalCount = 0;

Span<char> buffer = stackalloc char[size];

// Horizontal
for (var v = 0; v < size; v++)
{
    var line = input.AsSpan().Slice(v * size, size);
    totalCount += XmasCount(line);
}

// Vertical
for (var h = 0; h < size; h++)
{
    for (var v = 0; v < size; v++)
    {
        buffer[v] = Idx(h, v);
    }

    totalCount += XmasCount(buffer);
}

//  N   X
//   N  OX
//    N O X
//     NO  X
//      N   X

// Diagonal TL -> BR
for (var startX = 1 - size; startX < size; startX++)
{
    var x = startX;
    var y = 0;
    var lineLength = 0;
    for (var i = 0; i < size; i++)
    {
        if (TryIdx(x, y) is { } chr)
        {
            buffer[lineLength++] = chr;
        }

        x += 1;
        y += 1;
    }

    totalCount += XmasCount(buffer[..lineLength]);
}

// Diagonal TR -> BL
for (var startX = 1 - size; startX < size; startX++)
{
    var x = startX;
    var y = size - 1;
    var lineLength = 0;
    for (var i = 0; i < size; i++)
    {
        if (TryIdx(x, y) is { } chr)
        {
            buffer[lineLength++] = chr;
        }

        x += 1;
        y -= 1;
    }

    totalCount += XmasCount(buffer[..lineLength]);
}

Console.WriteLine($"Result: {totalCount}");

return;

char? TryIdx(int x, int y)
{
    if (x < 0 || x >= size || y < 0 || y >= size)
        return null;

    return Idx(x, y);
}

char Idx(int x, int y) => input[y * size + x];

int XmasCount(ReadOnlySpan<char> line)
{
    return xmasRegex.Count(line) + inverseXmasRegex.Count(line);
}

char[] ToGrid(string text)
{
    return text.ReplaceLineEndings("").ToCharArray();
}