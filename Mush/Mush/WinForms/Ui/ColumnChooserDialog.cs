using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Mush.Infrastructure.Config;
using Mush.AppLayer.Ports;

namespace Mush.WinForms.Ui
{
    public sealed class ColumnChooserDialog : Form
    {
        private readonly DataGridView _myc, _sp, _bu;
        private readonly IColumnsConfig _cfg;
        private readonly ITextService _t;

        private readonly TabControl tabs = new() { Dock = DockStyle.Fill };
        private CheckedListBox lbMyc = new() { Dock = DockStyle.Fill };
        private CheckedListBox lbSp = new() { Dock = DockStyle.Fill };
        private CheckedListBox lbBu = new() { Dock = DockStyle.Fill };

        public ColumnChooserDialog(DataGridView mycelium, DataGridView spawn, DataGridView bulk,
                                   IColumnsConfig cfg, ITextService t)
        {
            _myc = mycelium; _sp = spawn; _bu = bulk; _cfg = cfg; _t = t;
            Text = _t.T("MainForm.ColumnsDialogTitle") ?? "Select columns";
            Width = 420; Height = 460;
            StartPosition = FormStartPosition.CenterParent;

            var pBottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 50, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) };
            var btnOk = new Button { Text = "OK", AutoSize = true };
            var btnCancel = new Button { Text = _t.T("SpawnDialog.Cancel") ?? "Cancel", AutoSize = true };
            btnOk.Click += (_, __) => { ApplyAndSave(); DialogResult = DialogResult.OK; };
            btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; };
            pBottom.Controls.AddRange(new Control[] { btnOk, btnCancel });

            var tp1 = new TabPage("Mycelium") { Padding = new Padding(8) };
            var tp2 = new TabPage("Spawn") { Padding = new Padding(8) };
            var tp3 = new TabPage("Bulk") { Padding = new Padding(8) };
            tp1.Controls.Add(lbMyc); tp2.Controls.Add(lbSp); tp3.Controls.Add(lbBu);
            tabs.TabPages.AddRange(new[] { tp1, tp2, tp3 });

            Controls.Add(tabs);
            Controls.Add(pBottom);

            Load += (_, __) => FillListsFromGrids();
        }

        private void FillListsFromGrids()
        {
            FillList("mycelium", _myc, lbMyc);
            FillList("spawn", _sp, lbSp);
            FillList("bulk", _bu, lbBu);
        }

        private void FillList(string key, DataGridView grid, CheckedListBox lb)
        {
            lb.Items.Clear();
            var vis = _cfg.GetVisibility(key); // key->bool
            foreach (DataGridViewColumn c in grid.Columns)
            {
                // pracujeme jen s těmi, co mají Tag = localization key
                var tag = c.Tag as string;
                if (string.IsNullOrWhiteSpace(tag)) continue;
                var text = c.HeaderText;
                bool isChecked = vis.TryGetValue(tag, out var v) ? v : c.Visible;
                lb.Items.Add(new Item(tag, text), isChecked);
            }
        }

        private void ApplyAndSave()
        {
            ApplyOne("mycelium", _myc, lbMyc);
            ApplyOne("spawn", _sp, lbSp);
            ApplyOne("bulk", _bu, lbBu);
            _cfg.Save();
        }

        private void ApplyOne(string gridKey, DataGridView grid, CheckedListBox lb)
        {
            var map = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (var obj in lb.Items)
            {
                var it = (Item)obj;
                bool checkedState = lb.GetItemChecked(lb.Items.IndexOf(obj));
                map[it.Key] = checkedState;

                // promítnout do gridu
                foreach (DataGridViewColumn col in grid.Columns)
                {
                    if ((col.Tag as string)?.Equals(it.Key, StringComparison.OrdinalIgnoreCase) == true)
                        col.Visible = checkedState;
                }
            }
            _cfg.SetVisibility(gridKey, map);
        }

        private sealed record Item(string Key, string Title)
        {
            public override string ToString() => Title;
        }
    }
}
