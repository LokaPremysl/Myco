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
using Mush.AppLayer.Services;
using Mush.Infrastructure.Stores;
using Mush.WinForms.Ui;
using Mush.Infrastructure.Localization;

namespace Mush.WinForms
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        private readonly IMycologyStore _store;
        private readonly JsonMycologyStore _persist;
        private readonly ITextService _t;
        private ComboBox cboLang;

        private readonly BindingSource bsMycelium = new();
        private readonly BindingSource bsSpawn = new();
        private readonly BindingSource bsBulk = new();

        private DataGridView gridMycelium = new();
        private DataGridView gridSpawn = new();
        private DataGridView gridBulk = new();

        private Button btnAddMycelium;
        private Button btnAddSpawn;
        private Button btnAddBulk;

        public MainForm(IMycologyStore store, ITextService t, JsonMycologyStore persist)
        {
            _store = store;
            _persist = persist;
            _t = t;
            InitializeComponent();
            BuildLayout();
            BindData();

            _t.LanguageChanged += (_, __) => RefreshTexts();
            // načtení po zobrazení
            this.Load += async (_, __) =>
            {
                try
                {
                    var data = await _persist.LoadAsync();
                    if (data.Count > 0 && _store is MycologyStore impl)
                        impl.ReplaceAll(data);
                    // refresh bindingů
                    bsMycelium.ResetBindings(false);
                    RebindSpawns();
                }
                catch (Exception ex)
                {
                    // volitelné: MessageBox.Show($"Load failed: {ex.Message}");
                }
            };

            // uložení při zavření
            this.FormClosing += async (_, __) =>
            {
                try
                {
                    await _persist.SaveAsync(_store.Myceliums);
                }
                catch (Exception ex)
                {
                    // volitelné: MessageBox.Show($"Save failed: {ex.Message}");
                }
            };
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

            GridColumnFactory.ApplyMyceliumColumns(gridMycelium, _t);
            GridColumnFactory.ApplySpawnColumns(gridSpawn, _t);
            GridColumnFactory.ApplyBulkColumns(gridBulk,_t);

            gridsPanel.Controls.Add(gridMycelium, 0, 0);
            gridsPanel.Controls.Add(gridSpawn, 1, 0);
            gridsPanel.Controls.Add(gridBulk, 2, 0);

            layout.Controls.Add(topPanel, 0, 0);
            layout.Controls.Add(gridsPanel, 0, 1);
            Controls.Add(layout);

            var btnLoad = new Button { Text = "Load", AutoSize = true };
            var btnSave = new Button { Text = "Save", AutoSize = true };

            btnLoad.Click += async (_, __) => await LoadFromDiskAsync();
            btnSave.Click += async (_, __) => await SaveToDiskAsync();

            topPanel.Controls.AddRange(new Control[] { btnLoad, btnSave });


            // --- jazykový přepínač ---
            cboLang = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 120 };
            cboLang.DisplayMember = "Value";
            cboLang.ValueMember = "Key";
            cboLang.Items.Add(new KeyValuePair<string, string>("cs", "Čeština"));
            cboLang.Items.Add(new KeyValuePair<string, string>("en", "English"));

            // výběr aktuálního jazyka
            for (int i = 0; i < cboLang.Items.Count; i++)
            {
                var kv = (KeyValuePair<string, string>)cboLang.Items[i]!;
                if (string.Equals(kv.Key, _t.CurrentLanguage, StringComparison.OrdinalIgnoreCase))
                {
                    cboLang.SelectedIndex = i;
                    break;
                }
            }
            if (cboLang.SelectedIndex < 0) cboLang.SelectedIndex = 0; // default

            cboLang.SelectedValueChanged += (_, __) =>
            {
                var kv = (KeyValuePair<string, string>)cboLang.SelectedItem!;
                _t.SetLanguage(kv.Key);
            };

            topPanel.Controls.Add(cboLang);

            RefreshTexts();

        }

        //private void RefreshTexts()
        //{
        //    // zachovej datasourcy, při přestavbě sloupců se ztratí
        //    var dsMyc = gridMycelium.DataSource;
        //    var dsSp = gridSpawn.DataSource;
        //    var dsBu = gridBulk.DataSource;

        //    Mush.WinForms.Ui.GridColumnFactory.ApplyMyceliumColumns(gridMycelium, _t);
        //    Mush.WinForms.Ui.GridColumnFactory.ApplySpawnColumns(gridSpawn, _t);
        //    Mush.WinForms.Ui.GridColumnFactory.ApplyBulkColumns(gridBulk, _t);

        //    gridMycelium.DataSource = dsMyc;
        //    gridSpawn.DataSource = dsSp;
        //    gridBulk.DataSource = dsBu;

        //    // tlačítka (klíče už máš v TextService)
        //    btnAddMycelium.Text = _t.T("MainForm.AddCultureButton");
        //    btnAddSpawn.Text = _t.T("MainForm.AddSpawnButton");
        //    btnAddBulk.Text = _t.T("MainForm.AddBulkButton");

        //    // volitelně titulek okna
        //    this.Text = _t.T("MainForm.Tab.Culture"); // nebo si přidej vlastní klíč: "MainForm.Title"
        //}

        private void RefreshTexts()
        {
            void UpdateHeaders(DataGridView g)
            {
                foreach (DataGridViewColumn col in g.Columns)
                {
                    if (col.Tag is string key)
                        col.HeaderText = _t.T(key);
                }
            }

            UpdateHeaders(gridMycelium);
            UpdateHeaders(gridSpawn);
            UpdateHeaders(gridBulk);

            // tlačítka
            btnAddMycelium.Text = _t.T("MainForm.AddCultureButton");
            btnAddSpawn.Text = _t.T("MainForm.AddSpawnButton");
            btnAddBulk.Text = _t.T("MainForm.AddBulkButton");

            // titulek okna (nebo si přidej vlastní klíč)
            this.Text = _t.T("MainForm.Tab.Culture");
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

        private async Task LoadFromDiskAsync(string? path = null)
        {
            try
            {
                var data = await _persist.LoadAsync(path);
                if (_store is Mush.AppLayer.Services.MycologyStore impl)
                {
                    impl.ReplaceAll(data);          // přehodí kořen a přestaví indexy
                }
                else
                {
                    // fallback: bez ReplaceAll – ručně vyměnit
                    _store.Myceliums.Clear();
                    foreach (var m in data) _store.Myceliums.Add(m);
                    // pokud máš RebuildIndexes jen v implementaci, tady to nejde volat
                }

                // refresh UI
                bsMycelium.ResetBindings(false);
                RebindSpawns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Load failed:\n{ex.Message}", "Mush", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SaveToDiskAsync(string? path = null)
        {
            try
            {
                await _persist.SaveAsync(_store.Myceliums, path);
                // volitelně potvrzení:
                // MessageBox.Show(this, "Saved.", "Mush");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Save failed:\n{ex.Message}", "Mush", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}