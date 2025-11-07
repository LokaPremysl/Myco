using System.Collections.Generic;

namespace Mush.Infrastructure.Config
{
    public interface IColumnsConfig
    {
        // gridKey: "mycelium" | "spawn" | "bulk"
        IDictionary<string, bool> GetVisibility(string gridKey);
        void SetVisibility(string gridKey, IDictionary<string, bool> visibleByKey);
        void Save();
        void Load();
    }
}
