using System.Runtime.InteropServices;
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

var counts = new Dictionary<int, int>();
foreach (var i in listB)
{
    ref var entry = ref CollectionsMarshal.GetValueRefOrAddDefault(counts, i, out _);

    entry += 1;
}

var totalDistance = listA.Sum(a => a * counts.GetValueOrDefault(a));

Console.WriteLine(totalDistance);