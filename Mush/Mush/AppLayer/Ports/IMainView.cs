using System;
using System.Windows.Forms;
using Mush.AppLayer.Dtos;

namespace Mush.AppLayer.Ports
{
    public interface IMainView
    {
        // Bind targets (grids v MainForm na to navážou DataSource)
        BindingSource MyceliumBinding { get; }
        BindingSource SpawnBinding { get; }
        BindingSource BulkBinding { get; }

        // Přístup k aktuálním výběrům z View (presenter si je vyžádá)
        MyceliumRow? CurrentMycelium();
        SpawnRow? CurrentSpawn();

        // UI texty (lokalizace / přepínání jazyků)
        void SetTitle(string title);
        void SetButtons(string addMyc, string addSpawn, string addBulk, string load, string save);
        void LocalizeHeaders(Func<string, string> T); // presenter zavolá a view přepíše hlavičky podle klíče v Tag

        // Životní cyklus
        event EventHandler ViewLoaded;
        event FormClosingEventHandler ViewClosing;

        bool Confirm(string title, string message);
        void SetDeleteButtonText(string text);

        // pomocné: která tabulka má fokus?
        bool IsMyceliumFocused();
        bool IsSpawnFocused();
        bool IsBulkFocused();
    }
}
