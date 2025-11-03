//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace Mush.WinForms
//{
//    public partial class MainForm : Form
//    {
//        public MainForm()
//        {
//            InitializeComponent();
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mush.AppLayer.Dtos;
using Mush.AppLayer.Ports;
using Mush.WinForms.Ui;

namespace Mush.WinForms
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        private readonly IMycologyStore _store;

        private readonly BindingSource bsMycelium = new();
        private readonly BindingSource bsSpawn = new();
        private readonly BindingSource bsBulk = new();

        private DataGridView gridMycelium = new();
        private DataGridView gridSpawn = new();
        private DataGridView gridBulk = new();

        private Button btnAddMycelium;
        private Button btnAddSpawn;
        private Button btnAddBulk;

        public MainForm(IMycologyStore store)
        {
            _store = store;
            InitializeComponent();
            BuildLayout();
            BindData();
        }

        private void BuildLayout()
        {
            Text = "MushApp – Evidence pěstování";
            Width = 1000;
            Height = 700;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
            };
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Horní panel s tlačítky
            var topPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(8),
                BackColor = Color.AliceBlue
            };

            btnAddMycelium = new Button { Text = "+ Mycelium", AutoSize = true };
            btnAddSpawn = new Button { Text = "+ Spawn", AutoSize = true };
            btnAddBulk = new Button { Text = "+ Bulk", AutoSize = true };

            btnAddMycelium.Click += (_, __) => AddMycelium();
            btnAddSpawn.Click += (_, __) => AddSpawn();
            btnAddBulk.Click += (_, __) => AddBulk();

            topPanel.Controls.AddRange(new Control[] { btnAddMycelium, btnAddSpawn, btnAddBulk });

            // Gridy
            var gridsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
            };
            gridsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            gridsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            gridsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));


            gridMycelium = CreateGrid();
            gridSpawn = CreateGrid();
            gridBulk = CreateGrid();

            GridColumnFactory.ApplyMyceliumColumns(gridMycelium /*, _t*/);
            GridColumnFactory.ApplySpawnColumns(gridSpawn /*, _t*/);
            GridColumnFactory.ApplyBulkColumns(gridBulk /*, _t*/);


            //// Mycelium grid columns
            //gridMycelium.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Mycelium",
            //    HeaderText = "Mycelium",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            //});

            //gridMycelium.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Origin",
            //    HeaderText = "Origin",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            //});

            //gridMycelium.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Date",
            //    HeaderText = "Date",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            //    DefaultCellStyle = { Format = "yyyy-MM-dd" }
            //});
            //gridMycelium.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Medium",
            //    HeaderText = "Medium",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            //});

            //gridMycelium.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Status",
            //    HeaderText = "Status",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            //});

            //gridMycelium.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Notes",
            //    HeaderText = "Notes",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            //});

            //// Spawn grid columns
            //gridSpawn.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Material",
            //    HeaderText = "Spawn",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            //});

            //gridSpawn.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Date",
            //    HeaderText = "Inokulated",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            //    DefaultCellStyle = { Format = "yyyy-MM-dd" }
            //});
            //gridSpawn.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Inoculum",
            //    HeaderText = "Inoculum",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            //});

            //gridSpawn.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Jars",
            //    HeaderText = "Jars/Bags",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            //});

            //gridSpawn.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Temperature",
            //    HeaderText = "Incubation temp",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            //});

            //gridSpawn.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Colonization",
            //    HeaderText = "Colonization (%)",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            //});

            //gridSpawn.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Notes",
            //    HeaderText = "Notes",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            //});

            //// Bulk grid columns
            //gridBulk.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Material",
            //    HeaderText = "Bulk (substrate)",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            //});

            //gridBulk.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Date",
            //    HeaderText = "Boxed",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            //    DefaultCellStyle = { Format = "yyyy-MM-dd" }
            //});
            //gridBulk.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Ratio",
            //    HeaderText = "Spawn : substrate",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            //});

            //gridBulk.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "SpawnAmount",
            //    HeaderText = "Spawn amount",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            //});

            //gridBulk.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Hydration",
            //    HeaderText = "Hydration",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            //});

            //gridBulk.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "FruitingStart",
            //    HeaderText = "Fruiting start",
            //    DefaultCellStyle = { Format = "yyyy-MM-dd" },
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            //});

            //gridBulk.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "FlushCount",
            //    HeaderText = "Flushes",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            //});

            //gridBulk.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "Notes",
            //    HeaderText = "Notes",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            //});

            gridsPanel.Controls.Add(gridMycelium, 0, 0);
            gridsPanel.Controls.Add(gridSpawn, 1, 0);
            gridsPanel.Controls.Add(gridBulk, 2, 0);

            layout.Controls.Add(topPanel, 0, 0);
            layout.Controls.Add(gridsPanel, 0, 1);
            Controls.Add(layout);
        }

        private DataGridView CreateGrid()
        {
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                ReadOnly = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,

                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,

                AllowUserToResizeRows = false,
                RowHeadersVisible = false,

                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                AllowUserToResizeColumns = true,

                RowTemplate = { Height = 28 },
            };

            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            return grid;
        }

        private void BindData()
        {
            bsMycelium.DataSource = _store.Myceliums;
            gridMycelium.DataSource = bsMycelium;

            gridMycelium.SelectionChanged += (_, __) => RebindSpawns();
            gridSpawn.SelectionChanged += (_, __) => RebindBulks();
        }

        private void RebindSpawns()
        {
            var selected = gridMycelium.CurrentRow?.DataBoundItem as MyceliumRow;
            bsSpawn.DataSource = selected?.Spawns ?? new BindingList<SpawnRow>();
            gridSpawn.DataSource = bsSpawn;
            RebindBulks();
        }

        private void RebindBulks()
        {
            var selected = gridSpawn.CurrentRow?.DataBoundItem as SpawnRow;
            bsBulk.DataSource = selected?.Bulks ?? new BindingList<BulkRow>();
            gridBulk.DataSource = bsBulk;
        }

        private void AddMycelium()
        {
            using var dlg = new InputDialog("Nové mycelium", "Zadej název mycelia:");
            if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.Value))
            {
                _store.AddMycelium(dlg.Value, DateTime.Today, "neznámý původ");
                bsMycelium.ResetBindings(false);
            }
        }

        //private void AddSpawn()
        //{
        //    var parent = gridMycelium.CurrentRow?.DataBoundItem as MyceliumRow;
        //    if (parent == null) return;

        //    using var dlg = new InputDialog("Nový Spawn", "Zadej materiál (např. pšenice):");
        //    if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.Value))
        //    {
        //        _store.AddSpawn(parent, dlg.Value, DateTime.Today);
        //        bsSpawn.ResetBindings(false);
        //    }
        //}

        private void AddSpawn()
        {
            var myc = gridMycelium.CurrentRow?.DataBoundItem as MyceliumRow;
            if (myc == null) return;

            using var dlg = new InputDialog("Nový Spawn", "Zadej materiál:");
            if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.Value))
            {
                _store.AddSpawn(myc.Id, dlg.Value, DateTime.Today);
                bsSpawn.ResetBindings(false);
            }
        }

        //private void AddBulk()
        //{
        //    var parent = gridSpawn.CurrentRow?.DataBoundItem as SpawnRow;
        //    if (parent == null) return;

        //    using var dlg = new InputDialog("Nový Bulk", "Zadej substrát (např. CVG):");
        //    if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.Value))
        //    {
        //        _store.AddBulk(parent, dlg.Value, DateTime.Today);
        //        bsBulk.ResetBindings(false);
        //    }
        //}

        private void AddBulk()
        {
            var sp = gridSpawn.CurrentRow?.DataBoundItem as SpawnRow;
            if (sp == null) return;

            using var dlg = new InputDialog("Nový Bulk", "Zadej substrát:");
            if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.Value))
            {
                _store.AddBulk(sp.Id, dlg.Value, DateTime.Today);
                bsBulk.ResetBindings(false);
            }
        }


    }
}