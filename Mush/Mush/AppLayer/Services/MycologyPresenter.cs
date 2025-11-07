using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mush.AppLayer.Dtos;
using Mush.AppLayer.Ports;
using Mush.Infrastructure.Localization;
using Mush.Infrastructure.Stores;

namespace Mush.AppLayer.Services
{
    public sealed class MycologyPresenter : IMycologyPresenter
    {
        private readonly IMycologyStore _store;          // data (Singleton)
        private readonly JsonMycologyStore _persist;     // perzistence (Singleton)
        private readonly ITextService _t;                // lokalizace (Singleton)

        private IMainView? _view;

        private enum SelectionKind { None, Bulk, Spawn, Mycelium }

        // Vlastní BindingSource (drží je presenter; view se jen připojí přes DataSource)
        private readonly BindingSource _bsMyc = new();
        private readonly BindingSource _bsSp = new();
        private readonly BindingSource _bsBu = new();

        public MycologyPresenter(IMycologyStore store, JsonMycologyStore persist, ITextService t)
        {
            _store = store;
            _persist = persist;
            _t = t;
        }

        public void Attach(IMainView view)
        {
            _view = view;

            // propojit bindingy
            _bsMyc.DataSource = _store.Myceliums;
            _view.MyceliumBinding.DataSource = _bsMyc;

            // změna výběru v Mycelium → přepojit Spawn
            _bsSp.DataSource = new BindingList<SpawnRow>();
            _view.SpawnBinding.DataSource = _bsSp;

            _bsBu.DataSource = new BindingList<BulkRow>();
            _view.BulkBinding.DataSource = _bsBu;

            // inicializovat texty + hlavičky
            ApplyTexts();

            // jazyk – automatický refresh
            _t.LanguageChanged += OnLanguageChanged;

            // lifecycle view
            _view.ViewLoaded += OnViewLoaded;
            _view.ViewClosing += OnViewClosing;

            // první rebinding podle výběru
            RebindSpawns();
            RebindBulks();
        }

        public void Detach()
        {
            if (_view != null)
            {
                _view.ViewLoaded -= OnViewLoaded;
                _view.ViewClosing -= OnViewClosing;
            }
            _t.LanguageChanged -= OnLanguageChanged;
            _view = null;
        }

        private void OnLanguageChanged(object? s, EventArgs e) => ApplyTexts();

        private void ApplyTexts()
        {
            if (_view == null) return;
            _view.SetTitle(_t.T("MainForm.Tab.Culture"));
            _view.SetButtons(
                _t.T("MainForm.AddCultureButton"),
                _t.T("MainForm.AddSpawnButton"),
                _t.T("MainForm.AddBulkButton"),
                "Load",
                "Save"
            );
            _view.LocalizeHeaders(_t.T);
            _view.SetDeleteButtonText(_t.T("MainForm.DeleteButton"));
        }

        private void OnViewLoaded(object? s, EventArgs e)
        {
            // při zobrazení můžeš rovnou načíst (nebo necháš na tlačítku Load)
            // _ = LoadAsync();
            RebindSpawns();
            RebindBulks();
        }

        private void OnViewClosing(object? s, FormClosingEventArgs e)
        {
            // volitelně autosave
            // _ = SaveAsync();
            Detach();
        }

        private void RebindSpawns()
        {
            var myc = _view?.CurrentMycelium();
            _bsSp.DataSource = myc?.Spawns ?? new BindingList<SpawnRow>();
            RebindBulks();
        }

        private void RebindBulks()
        {
            var sp = _view?.CurrentSpawn();
            _bsBu.DataSource = sp?.Bulks ?? new BindingList<BulkRow>();
        }

        public void AddMycelium(string name)
        {
            _store.AddMycelium(name, DateTime.Today, "unknown");
            _bsMyc.ResetBindings(false);
            RebindSpawns();
        }

        public void AddSpawn(string material)
        {
            var myc = _view?.CurrentMycelium();
            if (myc is null) return;
            _store.AddSpawn(myc.Id, material, DateTime.Today);
            _bsSp.ResetBindings(false);
            RebindBulks();
        }

        public void AddBulk(string material)
        {
            var sp = _view?.CurrentSpawn();
            if (sp is null) return;
            _store.AddBulk(sp.Id, material, DateTime.Today);
            _bsBu.ResetBindings(false);
        }

        public async Task LoadAsync(CancellationToken ct = default)
        {
            var data = await _persist.LoadAsync(null, ct);
            if (_store is MycologyStore impl) impl.ReplaceAll(data);
            else
            {
                _store.Myceliums.Clear();
                foreach (var m in data) _store.Myceliums.Add(m);
            }
            _bsMyc.ResetBindings(false);
            RebindSpawns();
        }

        public void OnMyceliumSelectionChanged(object? row)
        {
            var myc = row as MyceliumRow ?? _view?.CurrentMycelium();
            _bsSp.DataSource = myc?.Spawns ?? new BindingList<SpawnRow>();
            _bsSp.ResetBindings(false);

            // zároveň přepočítat bulks podle nového spawnu (aktuální řádek ve spawn gridu po přebindu bývá null)
            _bsBu.DataSource = new BindingList<BulkRow>();
            _bsBu.ResetBindings(false);
        }

        public void OnSpawnSelectionChanged(object? row)
        {
            var sp = row as SpawnRow ?? _view?.CurrentSpawn();
            _bsBu.DataSource = sp?.Bulks ?? new BindingList<BulkRow>();
            _bsBu.ResetBindings(false);
        }

        public Task SaveAsync(CancellationToken ct = default)
            => _persist.SaveAsync(_store.Myceliums, null, ct);

        public Task DeleteSelectedAsync()
        {
            switch (GetSelectionKind())
            {
                case SelectionKind.Bulk: return TryDeleteBulkAsync();
                case SelectionKind.Spawn: return TryDeleteSpawnAsync();
                case SelectionKind.Mycelium: return TryDeleteMyceliumAsync();
                default: return Task.CompletedTask;
            }
        }

        // !!! NEJDŘÍV Bulk, pak Spawn, až pak Mycelium
        private SelectionKind GetSelectionKind()
        {
            if (_view?.BulkBinding?.Current is BulkRow) return SelectionKind.Bulk;
            if (_view?.SpawnBinding?.Current is SpawnRow) return SelectionKind.Spawn;
            if (_view?.CurrentMycelium() is MyceliumRow) return SelectionKind.Mycelium;
            return SelectionKind.None;
        }

        private async Task TryDeleteBulkAsync()
        {
            var sp = _view?.CurrentSpawn(); if (sp is null) return;
            var bulk = _view!.BulkBinding.Current as BulkRow; if (bulk is null) return;

            if (!Confirm("Confirm.DeleteBulk")) return;

            // Ideálně přes store: _store.RemoveBulk(sp.Id, bulk.Id);
            sp.Bulks.Remove(bulk);
            _bsBu.ResetBindings(false);
            await AfterDeleteAsync();
        }

        private async Task TryDeleteSpawnAsync()
        {
            var myc = _view?.CurrentMycelium(); if (myc is null) return;
            var sp = _view!.SpawnBinding.Current as SpawnRow; if (sp is null) return;

            var bulkCount = sp.Bulks?.Count ?? 0;
            if (!Confirm("Confirm.DeleteSpawn", bulkCount)) return;

            // Ideálně přes store: _store.RemoveSpawn(myc.Id, sp.Id);
            myc.Spawns.Remove(sp);

            _bsSp.ResetBindings(false);
            _bsBu.DataSource = new BindingList<BulkRow>();
            _bsBu.ResetBindings(false);
            await AfterDeleteAsync();
        }

        private async Task TryDeleteMyceliumAsync()
        {
            var myc = _view?.CurrentMycelium(); if (myc is null) return;

            var spawns = myc.Spawns?.Count ?? 0;
            var bulks = myc.Spawns?.Sum(s => s.Bulks?.Count ?? 0) ?? 0;
            if (!Confirm("Confirm.DeleteMycelium", spawns, bulks)) return;

            // Ideálně přes store: _store.RemoveMycelium(myc.Id);
            _store.Myceliums.Remove(myc);

            _bsMyc.ResetBindings(false);
            _bsSp.DataSource = new BindingList<SpawnRow>(); _bsSp.ResetBindings(false);
            _bsBu.DataSource = new BindingList<BulkRow>(); _bsBu.ResetBindings(false);
            await AfterDeleteAsync();
        }

        private bool Confirm(string key, params object[] args)
        {
            var title = _t.T("Dialog.Confirm");
            var msg = args.Length > 0 ? string.Format(_t.T(key), args) : _t.T(key);
            return _view!.Confirm(title, msg);
        }

        private Task AfterDeleteAsync() => Task.CompletedTask;

    }
}