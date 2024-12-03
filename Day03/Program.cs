using System.Text.RegularExpressions;
using Day03;

var input = Input.Text;
var regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");

Console.WriteLine(regex.Matches(input).Select(match =>
{
    Console.WriteLine(match.Value);
    var a = int.Parse(match.Groups[1].Value);
    var b = int.Parse(match.Groups[2].Value);
    return (long)(a * b);
}).Sum());
