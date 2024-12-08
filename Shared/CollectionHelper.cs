using System.Runtime.InteropServices;

namespace Shared;

public static class CollectionHelper
{
    public static TValue GetValueOrNew<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        where TValue : new() where TKey : notnull
    {
        ref var field = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
        if (!exists)
            field = new TValue();

        return field!;
    }
}