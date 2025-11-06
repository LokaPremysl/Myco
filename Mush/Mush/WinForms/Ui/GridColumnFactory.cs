using System.Windows.Forms;
using Mush.AppLayer.Dtos;
using Mush.AppLayer.Ports;

namespace Mush.WinForms.Ui
{
    // GridColumnFactory ukládá lokalizační klíč do Tag → MainForm.RefreshTexts()
    // pak jen přepíše HeaderText bez bourání sloupců
    internal static class GridColumnFactory
    {
        private static DataGridViewTextBoxColumn Col(
    string key, string fallback, string prop,
    ITextService? t,
    DataGridViewAutoSizeColumnMode size = DataGridViewAutoSizeColumnMode.AllCells,
    int? fillWeight = null, string? format = null)
        {
            var c = new DataGridViewTextBoxColumn
            {
                Tag = key, // <- uložíme lokalizační klíč
                HeaderText = t?.T(key) ?? fallback,
                DataPropertyName = prop,
                AutoSizeMode = size
            };
            if (fillWeight.HasValue && size == DataGridViewAutoSizeColumnMode.Fill)
                c.FillWeight = fillWeight.Value;
            if (!string.IsNullOrWhiteSpace(format))
                c.DefaultCellStyle.Format = format;
            return c;
        }

        public static void ApplyMyceliumColumns(DataGridView grid, ITextService? t = null)
        {
            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();

            grid.Columns.Add(Col("Main.Col.Mycelium", "Mycelium", nameof(MyceliumRow.Mycelium), t,
                                 DataGridViewAutoSizeColumnMode.Fill, 28));
            grid.Columns.Add(Col("Main.Col.Origin", "Origin", nameof(MyceliumRow.Origin), t,
                                 DataGridViewAutoSizeColumnMode.Fill, 22));
            grid.Columns.Add(Col("Main.Col.Medium", "Medium", nameof(MyceliumRow.Medium), t));
            grid.Columns.Add(Col("Main.Col.Status", "Status", nameof(MyceliumRow.Status), t));
            grid.Columns.Add(Col("Main.Col.Date", "Date", nameof(MyceliumRow.Date), t,
                                 DataGridViewAutoSizeColumnMode.AllCells, null, "yyyy-MM-dd"));
            grid.Columns.Add(Col("Main.Col.Notes", "Notes", nameof(MyceliumRow.Notes), t,
                                 DataGridViewAutoSizeColumnMode.Fill, 28));
        }

        public static void ApplySpawnColumns(DataGridView grid, ITextService? t = null)
        {
            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();

            //string T(string k, string f) => t?.T(k) ?? f;

            grid.Columns.Add(Col("Spawn.Col.Material", "Spawn", nameof(SpawnRow.Material), t,
                                 DataGridViewAutoSizeColumnMode.Fill, 26));
            grid.Columns.Add(Col("Spawn.Col.Inoculum", "Inoculum", nameof(SpawnRow.Inoculum), t));
            grid.Columns.Add(Col("Spawn.Col.Jars", "Jars/Bags", nameof(SpawnRow.Jars), t));
            grid.Columns.Add(Col("Spawn.Col.Temperature", "Incubation temp", nameof(SpawnRow.Temperature), t));
            grid.Columns.Add(Col("Spawn.Col.Colonization", "Colonization (%)", nameof(SpawnRow.Colonization), t));
            grid.Columns.Add(Col("Spawn.Col.Date", "Inoculated at", nameof(SpawnRow.Date), t,
                                 DataGridViewAutoSizeColumnMode.AllCells, null, "yyyy-MM-dd"));
            grid.Columns.Add(Col("Spawn.Col.Notes", "Notes", nameof(SpawnRow.Notes), t,
                                 DataGridViewAutoSizeColumnMode.Fill, 26));
        }

        public static void ApplyBulkColumns(DataGridView grid, ITextService? t = null)
        {
            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();

            //string T(string k, string f) => t?.T(k) ?? f;

            grid.Columns.Add(Col("Bulk.Col.Material", "Bulk (substrate)", nameof(BulkRow.Material), t,
                                 DataGridViewAutoSizeColumnMode.Fill, 25));
            grid.Columns.Add(Col("Bulk.Col.Ratio", "Spawn : substrate", nameof(BulkRow.Ratio), t));
            grid.Columns.Add(Col("Bulk.Col.SpawnAmount", "Spawn amount", nameof(BulkRow.SpawnAmount), t));
            grid.Columns.Add(Col("Bulk.Col.Hydration", "Hydration", nameof(BulkRow.Hydration), t));
            grid.Columns.Add(Col("Bulk.Col.FruitingStart", "Fruiting start", nameof(BulkRow.FruitingStart), t,
                                 DataGridViewAutoSizeColumnMode.AllCells, null, "yyyy-MM-dd"));
            grid.Columns.Add(Col("Bulk.Col.FlushCount", "Flushes", nameof(BulkRow.FlushCount), t));
            grid.Columns.Add(Col("Bulk.Col.Date", "Prepared at", nameof(BulkRow.Date), t,
                                 DataGridViewAutoSizeColumnMode.AllCells, null, "yyyy-MM-dd"));
            grid.Columns.Add(Col("Bulk.Col.Notes", "Notes", nameof(BulkRow.Notes), t,
                                 DataGridViewAutoSizeColumnMode.Fill, 25));
        }
    }
}
