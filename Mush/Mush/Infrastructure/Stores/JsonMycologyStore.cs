using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mush.AppLayer.Dtos;

namespace Mush.Infrastructure.Stores
{
    public sealed class JsonMycologyStore
    {
        private static readonly JsonSerializerOptions _json = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public string GetDefaultPath()
        {
            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Mush");
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, "mush.json");
        }

        public async Task SaveAsync(BindingList<MyceliumRow> data, string? path = null, CancellationToken ct = default)
        {
            path ??= GetDefaultPath();
            // BindingList -> List kvůli jednodušší serializaci (není nutné, ale čistší)
            var list = data.ToList();
            using var fs = File.Create(path);
            await JsonSerializer.SerializeAsync(fs, list, _json, ct);
        }

        public async Task<BindingList<MyceliumRow>> LoadAsync(string? path = null, CancellationToken ct = default)
        {
            path ??= GetDefaultPath();
            if (!File.Exists(path))
                return new BindingList<MyceliumRow>();

            await using var fs = File.OpenRead(path);
            var list = await JsonSerializer.DeserializeAsync<List<MyceliumRow>>(fs, _json, ct)
                       ?? new List<MyceliumRow>();
            return new BindingList<MyceliumRow>(list);
        }
    }
}
