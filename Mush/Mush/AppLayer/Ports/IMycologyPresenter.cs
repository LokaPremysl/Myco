using System.Threading;
using System.Threading.Tasks;

namespace Mush.AppLayer.Ports
{
    public interface IMycologyPresenter
    {
        void Attach(IMainView view);     // naváže view (a nastaví bindingy, texty, eventy)
        void Detach();                   // uvolní eventy

        // Příkazy z UI
        void AddMycelium(string name);
        void AddSpawn(string material);
        void AddBulk(string material);

        // Perzistence
        Task LoadAsync(CancellationToken ct = default);
        Task SaveAsync(CancellationToken ct = default);

        void OnMyceliumSelectionChanged(object? row);
        void OnSpawnSelectionChanged(object? row);
        Task DeleteSelectedAsync();
    }
}