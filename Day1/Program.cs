using System.Text.RegularExpressions;
using Day1;

var input = Input.Text();

var regex = new Regex(@"^\s*(\d+)\s+(\d+)\s*$", RegexOptions.Multiline);

var listA = new List<int>();
var listB = new List<int>();

foreach (Match match in regex.Matches(input))
{
    var a = int.Parse(match.Groups[1].Value);
    var b = int.Parse(match.Groups[2].Value);

    listA.Add(a);
    listB.Add(b);
}

listA.Sort();
listB.Sort();

long totalDistance = 0;
for (var i = 0; i < listA.Count; i++)
{
    var a = listA[i];
    var b = listB[i];

    var distance = Math.Abs(a - b);

    Console.WriteLine($"{a} dist {b} = {distance}");

    totalDistance += distance;
}

Console.WriteLine(totalDistance);
