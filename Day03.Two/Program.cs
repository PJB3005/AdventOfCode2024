using System.Text.RegularExpressions;
using Day03;

var input = Input.Text;
var regex = new Regex(@"(?:mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\))");

// This is horrible abuse of LINQ, but it's not like I'm the first person to commit it,
// so it's not like Microsoft can ever break it!
var enabled = true;

Console.WriteLine(regex.Matches(input).Select(match =>
{
    if (match.Value == "do()")
    {
        Console.WriteLine("do()");
        enabled = true;
        return 0;
    }

    if (match.Value == "don't()")
    {
        Console.WriteLine("don't()");
        enabled = false;
        return 0;
    }

    if (!enabled)
        return 0;

    Console.WriteLine(match.Value);
    var a = int.Parse(match.Groups[1].Value);
    var b = int.Parse(match.Groups[2].Value);
    return (long)(a * b);
}).Sum());
