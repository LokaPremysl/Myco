using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Mush.Infrastructure.Config
{
    public sealed class ColumnsConfig : IColumnsConfig
    {
        private readonly string _filePath;
        private readonly Dictionary<string, Dictionary<string, bool>> _map =
            new(StringComparer.OrdinalIgnoreCase);

        public ColumnsConfig(string? baseFolder = null, string fileName = "columns.json")
        {
            baseFolder ??= Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MushApp");
            Directory.CreateDirectory(baseFolder);
            _filePath = Path.Combine(baseFolder, fileName);
            Load();
        }

        public IDictionary<string, bool> GetVisibility(string gridKey)
        {
            if (!_map.TryGetValue(gridKey, out var v))
                _map[gridKey] = v = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            return v;
        }

        public void SetVisibility(string gridKey, IDictionary<string, bool> visibleByKey)
        {
            _map[gridKey] = new Dictionary<string, bool>(visibleByKey, StringComparer.OrdinalIgnoreCase);
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(_map, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void Load()
        {
            if (!File.Exists(_filePath)) return;
            try
            {
                var json = File.ReadAllText(_filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, bool>>>(json)
                           ?? new();
                _map.Clear();
                foreach (var kv in data) _map[kv.Key] = kv.Value ?? new();
            }
            catch
            {
                // ignore broken file
            }
        }
    }
}