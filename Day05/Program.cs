using Shared;

var input = Input.Get("day05");
var (deps, updates) = ParseInput(input);

var keyedDeps = deps.GroupBy(k => k.before).ToDictionary(g => g.Key, g => g.Select(dep => dep.after).ToHashSet());

var sumMiddlePages = 0;
var sumFixedMiddlePages = 0;

foreach (var update in updates)
{
    if (IsOrderedProperly(update, keyedDeps, out _, out _))
    {
        Console.WriteLine($"Valid: {string.Join(',', update.Select(i => i.ToString()))}");

        sumMiddlePages += update[update.Length / 2];
    }
    else
    {
        FixOrdering(update, keyedDeps);

        sumFixedMiddlePages += update[update.Length / 2];
    }
}

Console.WriteLine($"SUM CORRECT: {sumMiddlePages}");
Console.WriteLine($"SUM FIXED: {sumFixedMiddlePages}");

return;

static void FixOrdering(int[] update, Dictionary<int, HashSet<int>> keyedDeps)
{
    while (!IsOrderedProperly(update, keyedDeps, out var badA, out var badB))
    {
        Swap<int>(update, badA, badB);
    }
}

static void Swap<T>(Span<T> span, int a, int b)
{
    (span[a], span[b]) = (span[b], span[a]);
}

static bool IsOrderedProperly(int[] update, Dictionary<int, HashSet<int>> keyedDeps, out int badA, out int badB)
{
    for (int i = 0; i < update.Length - 1; i++)
    {
        var page = update[i];

        badA = i;
        if (!IsOrderedFrom(page, update.AsSpan(i + 1), keyedDeps, out var badBSlot))
        {
            badB = badBSlot + i + 1;
            return false;
        }
    }

    badA = default;
    badB = default;
    return true;
}

static bool IsOrderedFrom(int update, ReadOnlySpan<int> from, Dictionary<int, HashSet<int>> keyedDeps, out int bad)
{
    for (var index = 0; index < from.Length; index++)
    {
        var i = from[index];
        if (keyedDeps.TryGetValue(i, out var before) && before.Contains(update))
        {
            bad = index;
            return false;
        }
    }

    bad = default;
    return true;
}

static (List<(int before, int after)>, List<int[]> updates) ParseInput(string text)
{
    var deps = new List<(int, int)>();
    var updates = new List<int[]>();

    var reader = new StringReader(text);

    // Deps
    while (reader.ReadLine()?.Trim() is { Length: > 1 } line)
    {
        var split = line.Split('|');

        var before = int.Parse(split[0]);
        var after = int.Parse(split[1]);

        deps.Add((before, after));
    }

    // Updates
    while (reader.ReadLine()?.Trim() is { Length: > 1 } line)
    {
        updates.Add(line.Split(',').Select(int.Parse).ToArray());
    }

    return (deps, updates);
}