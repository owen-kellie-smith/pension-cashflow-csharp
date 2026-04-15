namespace PensionModel.IO;
using PensionModel.Models;

public class MortalityProvider
{
    private readonly Dictionary<string, List<MortalityRow>> _cache = new();

    public List<MortalityRow> Get(string key, string folder)
    {
        if (!_cache.TryGetValue(key, out var mortality))
        {
            mortality = MortalityReader.Read(key, folder);
            _cache[key] = mortality;
        }

        return mortality;
    }
}
