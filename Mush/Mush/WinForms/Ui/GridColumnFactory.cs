using System.Windows.Forms;
using Mush.AppLayer.Dtos;
using Mush.AppLayer.Ports;

namespace Mush.WinForms.Ui
{
    internal static class GridColumnFactory
    {
        // Malý helper na textový sloupec
        //private static DataGridViewTextBoxColumn Col(
        //    string header, string prop,
        //    DataGridViewAutoSizeColumnMode size = DataGridViewAutoSizeColumnMode.AllCells,
        //    int? fillWeight = null, string? format = null)
        //{
        //    var c = new DataGridViewTextBoxColumn
        //    {
        //        HeaderText = header,
        //        DataPropertyName = prop,
        //        AutoSizeMode = size
        //    };
        //    if (fillWeight.HasValue && size == DataGridViewAutoSizeColumnMode.Fill)
        //        c.FillWeight = fillWeight.Value;
        //    if (!string.IsNullOrWhiteSpace(format))
        //        c.DefaultCellStyle.Format = format;
        //    return c;
        //}

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

        // Mycelium
        //public static void ApplyMyceliumColumns(DataGridView grid, ITextService? t = null)
        //{
        //    grid.AutoGenerateColumns = false;
        //    grid.Columns.Clear();

        //    string T(string k, string fallback) => t?.T(k) ?? fallback;

        //    grid.Columns.Add(Col(T("Main.Col.Mycelium", "Mycelium"), nameof(MyceliumRow.Mycelium),
        //                         DataGridViewAutoSizeColumnMode.Fill, 28));
        //    grid.Columns.Add(Col(T("Main.Col.Origin", "Origin"), nameof(MyceliumRow.Origin),
        //                         DataGridViewAutoSizeColumnMode.Fill, 22));
        //    grid.Columns.Add(Col(T("Main.Col.Medium", "Medium"), nameof(MyceliumRow.Medium)));
        //    grid.Columns.Add(Col(T("Main.Col.Status", "Status"), nameof(MyceliumRow.Status)));
        //    grid.Columns.Add(Col(T("Main.Col.Date", "Date"), nameof(MyceliumRow.Date),
        //                         DataGridViewAutoSizeColumnMode.AllCells, null, "yyyy-MM-dd"));
        //    grid.Columns.Add(Col(T("Main.Col.Notes", "Notes"), nameof(MyceliumRow.Notes),
        //                         DataGridViewAutoSizeColumnMode.Fill, 28));
        //}

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

        // Spawn
        //public static void ApplySpawnColumns(DataGridView grid, ITextService? t = null)
        //{
        //    grid.AutoGenerateColumns = false;
        //    grid.Columns.Clear();

        //    //string T(string k, string f) => t?.T(k) ?? f;

        //    grid.Columns.Add(Col("Spawn.Col.Material", "Spawn", nameof(SpawnRow.Material), t,
        //                         DataGridViewAutoSizeColumnMode.Fill, 26));
        //    grid.Columns.Add(Col(T("Spawn.Col.Inoculum", "Inoculum"), nameof(SpawnRow.Inoculum)));
        //    grid.Columns.Add(Col(T("Spawn.Col.Jars", "Jars/Bags"), nameof(SpawnRow.Jars)));
        //    grid.Columns.Add(Col(T("Spawn.Col.Temperature", "Incubation temp"), nameof(SpawnRow.Temperature)));
        //    grid.Columns.Add(Col(T("Spawn.Col.Colonization", "Colonization (%)"), nameof(SpawnRow.Colonization)));
        //    grid.Columns.Add(Col(T("Spawn.Col.Date", "Inoculated at"), nameof(SpawnRow.Date),
        //                         DataGridViewAutoSizeColumnMode.AllCells, null, "yyyy-MM-dd"));
        //    grid.Columns.Add(Col(T("Spawn.Col.Notes", "Notes"), nameof(SpawnRow.Notes),
        //                         DataGridViewAutoSizeColumnMode.Fill, 26));
        //}

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

        // Bulk
        //public static void ApplyBulkColumns(DataGridView grid, ITextService? t = null)
        //{
        //    grid.AutoGenerateColumns = false;
        //    grid.Columns.Clear();

        //    string T(string k, string f) => t?.T(k) ?? f;

        //    grid.Columns.Add(Col(T("Bulk.Col.Material", "Bulk (substrate)"), nameof(BulkRow.Material),
        //                         DataGridViewAutoSizeColumnMode.Fill, 25));
        //    grid.Columns.Add(Col(T("Bulk.Col.Ratio", "Spawn : substrate"), nameof(BulkRow.Ratio)));
        //    grid.Columns.Add(Col(T("Bulk.Col.SpawnAmount", "Spawn amount"), nameof(BulkRow.SpawnAmount)));
        //    grid.Columns.Add(Col(T("Bulk.Col.Hydration", "Hydration"), nameof(BulkRow.Hydration)));
        //    grid.Columns.Add(Col(T("Bulk.Col.FruitingStart", "Fruiting start"), nameof(BulkRow.FruitingStart),
        //                         DataGridViewAutoSizeColumnMode.AllCells, null, "yyyy-MM-dd"));
        //    grid.Columns.Add(Col(T("Bulk.Col.FlushCount", "Flushes"), nameof(BulkRow.FlushCount)));
        //    grid.Columns.Add(Col(T("Bulk.Col.Date", "Prepared at"), nameof(BulkRow.Date),
        //                         DataGridViewAutoSizeColumnMode.AllCells, null, "yyyy-MM-dd"));
        //    grid.Columns.Add(Col(T("Bulk.Col.Notes", "Notes"), nameof(BulkRow.Notes),
        //                         DataGridViewAutoSizeColumnMode.Fill, 25));
        //}

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
