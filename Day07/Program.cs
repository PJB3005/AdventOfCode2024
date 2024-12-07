using System.Diagnostics;
using Shared;

var sw = Stopwatch.StartNew();
var input = Input.Get("day07");
var equations = ParseInput(input);

var totalSum = equations.AsParallel().Where(IsEquationPossible).Sum(eq => eq.Result);

Console.WriteLine($"Total sum: {totalSum}");
Console.WriteLine(sw.Elapsed);

return;

static bool IsEquationPossible(Equation equation)
{
    var operatorCount = equation.Terms.Length - 1;
    var operatorPermutationCount = 2 << operatorCount;

    for (uint opBits = 0; opBits < operatorPermutationCount; opBits++)
    {
        var result = EvaluateEquation(equation, opBits);
        if (result == equation.Result)
            return true;
    }

    return false;
}

static long EvaluateEquation(Equation equation, uint operatorBits)
{
    long result = equation.Terms[0];

    for (var i = 1; i < equation.Terms.Length; i++)
    {
        var term = equation.Terms[i];
        if (IdxBit(operatorBits, i))
        {
            result *= term;
        }
        else
        {
            result += term;
        }
    }

    return result;
}

static bool IdxBit(uint bits, int index)
{
    return (bits & (1 << index)) != 0;
}

static List<Equation> ParseInput(string input)
{
    var equations = new List<Equation>();
    var reader = new StringReader(input);
    while (reader.ReadLine() is { } line)
    {
        var contents = line.Trim();
        var splitColon = contents.Split(':');

        var result = long.Parse(splitColon[0]);
        var terms = splitColon[1].Trim().Split(' ').Select(int.Parse).ToArray();

        equations.Add(new Equation(result, terms));
    }

    return equations;
}

internal record struct Equation(long Result, int[] Terms);