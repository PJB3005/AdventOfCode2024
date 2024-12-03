using System.Runtime.InteropServices;
using Day2;

var input = Input.Text;
var reader = new StringReader(input);

var list = new List<int[]>();
while (reader.ReadLine() is { } line)
{
    list.Add(line.Trim().Split(' ').Select(int.Parse).ToArray());
}

Console.WriteLine(list.Where(IsReportSafeForReal).Count());

return;

bool IsReportSafeForReal(int[] report)
{
    if (IsReportSafe(report))
        return true;

    for (var i = 0; i < report.Length; i++)
    {
        var reportList = report.ToList();
        reportList.RemoveAt(i);
        if (IsReportSafe(CollectionsMarshal.AsSpan(reportList)))
            return true;
    }

    return false;
}

bool IsReportSafe(ReadOnlySpan<int> report)
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