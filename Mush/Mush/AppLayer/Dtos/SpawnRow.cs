using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mush.AppLayer.Dtos
{
    public sealed class SpawnRow : INotifyPropertyChanged
    {
        public Guid MyceliumId { get; set; }
        private string _material = string.Empty;
        private DateTime _date = DateTime.Today;

        // NOVÉ:
        private string _inoculum = "";         // agar / LC / G2G / přímý zdroj
        private string _jars = "";             // např. "6× 720 ml" nebo počet kusů
        private string _temperature = "";      // inkubační teplota (textem)
        private int _colonization = 0;         // % kolonizace
        private string _notes = "";
        
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Material { get => _material; set { if (_material == value) return; _material = value; OnChanged(nameof(Material)); } }
        public DateTime Date { get => _date; set { if (_date == value) return; _date = value; OnChanged(nameof(Date)); } }

        public string Inoculum { get => _inoculum; set { if (_inoculum == value) return; _inoculum = value; OnChanged(nameof(Inoculum)); } }
        public string Jars { get => _jars; set { if (_jars == value) return; _jars = value; OnChanged(nameof(Jars)); } }
        public string Temperature { get => _temperature; set { if (_temperature == value) return; _temperature = value; OnChanged(nameof(Temperature)); } }
        public int Colonization { get => _colonization; set { if (_colonization == value) return; _colonization = value; OnChanged(nameof(Colonization)); } }
        public string Notes { get => _notes; set { if (_notes == value) return; _notes = value; OnChanged(nameof(Notes)); } }

        public BindingList<BulkRow> Bulks { get; set; } = new();
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnChanged(string p) => PropertyChanged?.Invoke(this, new(p));


        //public string Material
        //{
        //    get => _material;
        //    set
        //    {
        //        { if (_material == value) return;
        //            _material = value;
        //            OnChanged(nameof(Material));
        //        }
        //    }
        //}

        //public DateTime Date
        //{
        //    get => _date;
        //    set
        //    {
        //        if (_date == value) return;
        //        _date = value;
        //        OnChanged(nameof(Date));
        //    }
        //}

        
    }
}
