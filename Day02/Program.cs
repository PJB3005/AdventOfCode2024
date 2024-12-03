using Day2;

var input = Input.Text;
var reader = new StringReader(input);

var list = new List<int[]>();
while (reader.ReadLine() is { } line)
{
    list.Add(line.Trim().Split(' ').Select(int.Parse).ToArray());
}

foreach (var report in list.Where(IsReportSafe))
{
    Console.WriteLine(string.Join(" ", report.Select(x => x.ToString())));
}

Console.WriteLine(list.Where(IsReportSafe).Count());

return;

bool IsReportSafe(int[] report)
{
    var sign = Math.Sign(report[0] - report[1]);

    var last = report[0];
    for (var i = 1; i < report.Length; i++)
    {
        var next = report[i];

        if (sign != Math.Sign(last - next))
            return false;

        var diff = Math.Abs(last - next);
        if (diff is not (1 or 2 or 3))
            return false;

        last = next;
    }

    return true;
}


