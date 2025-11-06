using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
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

        public Task SaveAsync(CancellationToken ct = default)
            => _persist.SaveAsync(_store.Myceliums, null, ct);
    }
}