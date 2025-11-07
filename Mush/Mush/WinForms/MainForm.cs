
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
    public partial class MainForm : Form, IMainView
    {
        private readonly IMycologyPresenter _p;
        private readonly ITextService _t;
        public BindingSource MyceliumBinding { get; } = new();
        public BindingSource SpawnBinding { get; } = new();
        public BindingSource BulkBinding { get; } = new();

        private DataGridView gridMycelium = new();
        private DataGridView gridSpawn = new();
        private DataGridView gridBulk = new();

        private Button btnAddMycelium, btnAddSpawn, btnAddBulk, btnLoad, btnSave, btnDelete;
        private ComboBox cboLang;

        public event EventHandler? ViewLoaded;
        public event FormClosingEventHandler? ViewClosing;

        

        public MainForm(IMycologyPresenter presenter, ITextService texts)
        {
            _p = presenter;
            _t = texts;

            InitializeComponent();
            BuildLayout();
            WireBindings();

            gridMycelium.SelectionChanged += (_, __) => UpdateDeleteButton();
            gridSpawn.SelectionChanged += (_, __) => UpdateDeleteButton();
            gridBulk.SelectionChanged += (_, __) => UpdateDeleteButton();
            UpdateDeleteButton();

            // připojit presenter až po vytvoření UI
            _p.Attach(this);

            // lifecycle do IMainView eventů
            this.Load += (_, __) => ViewLoaded?.Invoke(this, EventArgs.Empty);
            this.FormClosing += (s, e) => ViewClosing?.Invoke(this, e);
            
            //Del na mazání
            this.KeyPreview = true;
            this.KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Delete)
                {
                    _p.DeleteSelectedAsync();
                    e.Handled = true;
                }
            };
        }

        void UpdateDeleteButton()
        {
            btnDelete.Enabled =
                gridBulk.CurrentRow != null ||
                gridSpawn.CurrentRow != null ||
                gridMycelium.CurrentRow != null;
        }
        

        private void BuildLayout()
        {
            Text = "MushApp";
            Width = 1000; Height = 700;

            var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(8) };
            btnAddMycelium = new Button { AutoSize = true };
            btnAddSpawn = new Button { AutoSize = true };
            btnAddBulk = new Button { AutoSize = true };
            btnLoad = new Button { Text = "Load", AutoSize = true };
            btnSave = new Button { Text = "Save", AutoSize = true };
            btnDelete = new Button { AutoSize = true };
            btnDelete.Click += (_, __) => _p.DeleteSelectedAsync();   // presenter rozhodne co

            top.Controls.Add(btnDelete);

            btnAddMycelium.Click += (_, __) =>
            {
                using var dlg = new InputDialog("New Mycelium", "Name:");
                if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.Value))
                    _p.AddMycelium(dlg.Value);
            };
            btnAddSpawn.Click += (_, __) =>
            {
                using var dlg = new InputDialog("New Spawn", "Material:");
                if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.Value))
                    _p.AddSpawn(dlg.Value);
            };
            btnAddBulk.Click += (_, __) =>
            {
                using var dlg = new InputDialog("New Bulk", "Substrate:");
                if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.Value))
                    _p.AddBulk(dlg.Value);
            };

            btnLoad.Click += async (_, __) => await _p.LoadAsync();
            btnSave.Click += async (_, __) => await _p.SaveAsync();

            // přepínač jazyka (volitelné – presenter umí LanguageChanged sám)
            cboLang = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 120 };
            cboLang.Items.Add(new KeyValuePair<string, string>("cs", "Čeština"));
            cboLang.Items.Add(new KeyValuePair<string, string>("en", "English"));
            cboLang.SelectedIndex = _t.CurrentLanguage.StartsWith("cs", StringComparison.OrdinalIgnoreCase) ? 0 : 1;
            cboLang.SelectedValueChanged += (_, __) =>
            {
                var kv = (KeyValuePair<string, string>)cboLang.SelectedItem!;
                _t.SetLanguage(kv.Key); // presenter zachytí LanguageChanged a přepíše texty
            };

            top.Controls.AddRange(new Control[] { btnAddMycelium, btnAddSpawn, btnAddBulk, btnLoad, btnSave, cboLang });

            var body = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 2 };
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            body.RowStyles.Add(new RowStyle(SizeType.Percent, 67));
            body.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            gridMycelium = CreateGrid();
            gridSpawn = CreateGrid();
            gridBulk = CreateGrid();

            GridColumnFactory.ApplyMyceliumColumns(gridMycelium, _t);
            GridColumnFactory.ApplySpawnColumns(gridSpawn, _t);
            GridColumnFactory.ApplyBulkColumns(gridBulk, _t);

            body.Controls.Add(gridMycelium, 0, 0);
            body.Controls.Add(gridSpawn, 1, 0);
            body.Controls.Add(gridBulk, 2, 0);

            Controls.Add(body);
            Controls.Add(top);

            
        }

        private DataGridView CreateGrid()
        {
            var g = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                ReadOnly = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false
            };
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            g.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            return g;
        }

        private void WireBindings()
        {
            gridMycelium.DataSource = MyceliumBinding;
            gridSpawn.DataSource = SpawnBinding;
            gridBulk.DataSource = BulkBinding;

            // při změně výběru jen požadavek na presenter (ten si výběr vyžádá přes Current* metody)
            gridMycelium.SelectionChanged += (_, __) =>
       _p.OnMyceliumSelectionChanged(gridMycelium.CurrentRow?.DataBoundItem);

            gridSpawn.SelectionChanged += (_, __) =>
                _p.OnSpawnSelectionChanged(gridSpawn.CurrentRow?.DataBoundItem);
        }

        // IMainView implementace
        public MyceliumRow? CurrentMycelium()
            => gridMycelium.CurrentRow?.DataBoundItem as MyceliumRow;

        public SpawnRow? CurrentSpawn()
            => gridSpawn.CurrentRow?.DataBoundItem as SpawnRow;

        public void SetTitle(string title) => this.Text = title;

        public void SetButtons(string addMyc, string addSpawn, string addBulk, string load, string save)
        {
            btnAddMycelium.Text = addMyc;
            btnAddSpawn.Text = addSpawn;
            btnAddBulk.Text = addBulk;
            btnLoad.Text = load;
            btnSave.Text = save;
        }

        public void LocalizeHeaders(Func<string, string> T)
        {
            void Apply(DataGridView g)
            {
                foreach (DataGridViewColumn c in g.Columns)
                    if (c.Tag is string key) c.HeaderText = T(key);
            }
            Apply(gridMycelium); Apply(gridSpawn); Apply(gridBulk);
        }

        public bool Confirm(string title, string message)
    => MessageBox.Show(this, message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;

        public void SetDeleteButtonText(string text) => btnDelete.Text = text;

        public bool IsMyceliumFocused() => gridMycelium.Focused || gridMycelium.ContainsFocus;
        public bool IsSpawnFocused() => gridSpawn.Focused || gridSpawn.ContainsFocus;
        public bool IsBulkFocused() => gridBulk.Focused || gridBulk.ContainsFocus;


    }
}