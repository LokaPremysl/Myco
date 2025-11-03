using System.Windows.Forms;
using Mush.AppLayer.Dtos;
using Mush.AppLayer.Ports;

namespace Mush.WinForms.Ui
{
    internal static class GridColumnFactory
    {
        // Malý helper na textový sloupec
        private static DataGridViewTextBoxColumn Col(
            string header, string prop,
            DataGridViewAutoSizeColumnMode size = DataGridViewAutoSizeColumnMode.AllCells,
            int? fillWeight = null, string? format = null)
        {
            var c = new DataGridViewTextBoxColumn
            {
                HeaderText = header,
                DataPropertyName = prop,
                AutoSizeMode = size
            };
            if (fillWeight.HasValue && size == DataGridViewAutoSizeColumnMode.Fill)
                c.FillWeight = fillWeight.Value;
            if (!string.IsNullOrWhiteSpace(format))
                c.DefaultCellStyle.Format = format;
            return c;
        }

        // Mycelium
        public static void ApplyMyceliumColumns(DataGridView grid, ITextService? t = null)
        {
            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();

            string T(string k, string fallback) => t?.T(k) ?? fallback;

            grid.Columns.Add(Col(T("Main.Col.Mycelium", "Mycelium"), nameof(MyceliumRow.Mycelium),
                                 DataGridViewAutoSizeColumnMode.Fill, 28));
            grid.Columns.Add(Col(T("Main.Col.Origin", "Origin"), nameof(MyceliumRow.Origin),
                                 DataGridViewAutoSizeColumnMode.Fill, 22));
            grid.Columns.Add(Col(T("Main.Col.Medium", "Medium"), nameof(MyceliumRow.Medium)));
            grid.Columns.Add(Col(T("Main.Col.Status", "Status"), nameof(MyceliumRow.Status)));
            grid.Columns.Add(Col(T("Main.Col.Date", "Date"), nameof(MyceliumRow.Date),
                                 DataGridViewAutoSizeColumnMode.AllCells, null, "yyyy-MM-dd"));
            grid.Columns.Add(Col(T("Main.Col.Notes", "Notes"), nameof(MyceliumRow.Notes),
                                 DataGridViewAutoSizeColumnMode.Fill, 28));
        }

        // Spawn
        public static void ApplySpawnColumns(DataGridView grid, ITextService? t = null)
        {
            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();

            string T(string k, string f) => t?.T(k) ?? f;

            grid.Columns.Add(Col(T("Spawn.Col.Material", "Spawn"), nameof(SpawnRow.Material),
                                 DataGridViewAutoSizeColumnMode.Fill, 26));
            grid.Columns.Add(Col(T("Spawn.Col.Inoculum", "Inoculum"), nameof(SpawnRow.Inoculum)));
            grid.Columns.Add(Col(T("Spawn.Col.Jars", "Jars/Bags"), nameof(SpawnRow.Jars)));
            grid.Columns.Add(Col(T("Spawn.Col.Temperature", "Incubation temp"), nameof(SpawnRow.Temperature)));
            grid.Columns.Add(Col(T("Spawn.Col.Colonization", "Colonization (%)"), nameof(SpawnRow.Colonization)));
            grid.Columns.Add(Col(T("Spawn.Col.Date", "Inoculated at"), nameof(SpawnRow.Date),
                                 DataGridViewAutoSizeColumnMode.AllCells, null, "yyyy-MM-dd"));
            grid.Columns.Add(Col(T("Spawn.Col.Notes", "Notes"), nameof(SpawnRow.Notes),
                                 DataGridViewAutoSizeColumnMode.Fill, 26));
        }

        // Bulk
        public static void ApplyBulkColumns(DataGridView grid, ITextService? t = null)
        {
            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();

            string T(string k, string f) => t?.T(k) ?? f;

            grid.Columns.Add(Col(T("Bulk.Col.Material", "Bulk (substrate)"), nameof(BulkRow.Material),
                                 DataGridViewAutoSizeColumnMode.Fill, 25));
            grid.Columns.Add(Col(T("Bulk.Col.Ratio", "Spawn : substrate"), nameof(BulkRow.Ratio)));
            grid.Columns.Add(Col(T("Bulk.Col.SpawnAmount", "Spawn amount"), nameof(BulkRow.SpawnAmount)));
            grid.Columns.Add(Col(T("Bulk.Col.Hydration", "Hydration"), nameof(BulkRow.Hydration)));
            grid.Columns.Add(Col(T("Bulk.Col.FruitingStart", "Fruiting start"), nameof(BulkRow.FruitingStart),
                                 DataGridViewAutoSizeColumnMode.AllCells, null, "yyyy-MM-dd"));
            grid.Columns.Add(Col(T("Bulk.Col.FlushCount", "Flushes"), nameof(BulkRow.FlushCount)));
            grid.Columns.Add(Col(T("Bulk.Col.Date", "Prepared at"), nameof(BulkRow.Date),
                                 DataGridViewAutoSizeColumnMode.AllCells, null, "yyyy-MM-dd"));
            grid.Columns.Add(Col(T("Bulk.Col.Notes", "Notes"), nameof(BulkRow.Notes),
                                 DataGridViewAutoSizeColumnMode.Fill, 25));
        }
    }
}
