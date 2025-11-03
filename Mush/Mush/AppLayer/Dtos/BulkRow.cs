using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mush.AppLayer.Dtos
{
    public sealed class BulkRow : INotifyPropertyChanged
    {
        public Guid SpawnId { get; set; }
        private string _material = string.Empty;
        private DateTime _date = DateTime.Today;

        // NOVÉ:
        private string _ratio = "";           // poměr spawn:substrát (např. 1:3)
        private string _hydration = "";       // hydratace (např. 60–65 %)
        private string _spawnAmount = "";     // kolik spawnu (textem nebo g)
        private DateTime? _fruitingStart;     // začátek plodování
        private int _flushCount = 0;          // počet flushů
        private string _notes = "";

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Material { get => _material; set { if (_material == value) return; _material = value; OnChanged(nameof(Material)); } }
        public DateTime Date { get => _date; set { if (_date == value) return; _date = value; OnChanged(nameof(Date)); } }

        public string Ratio { get => _ratio; set { if (_ratio == value) return; _ratio = value; OnChanged(nameof(Ratio)); } }
        public string Hydration { get => _hydration; set { if (_hydration == value) return; _hydration = value; OnChanged(nameof(Hydration)); } }
        public string SpawnAmount { get => _spawnAmount; set { if (_spawnAmount == value) return; _spawnAmount = value; OnChanged(nameof(SpawnAmount)); } }
        public DateTime? FruitingStart { get => _fruitingStart; set { if (_fruitingStart == value) return; _fruitingStart = value; OnChanged(nameof(FruitingStart)); } }
        public int FlushCount { get => _flushCount; set { if (_flushCount == value) return; _flushCount = value; OnChanged(nameof(FlushCount)); } }
        public string Notes { get => _notes; set { if (_notes == value) return; _notes = value; OnChanged(nameof(Notes)); } }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnChanged(string p) => PropertyChanged?.Invoke(this, new(p));

        //public string Material
        //{ 
        //    get => _material; 
        //    set 
        //    {
        //        if(_material ==  value) return;
        //        _material = value;
        //        OnChanged(nameof(Material));
        //    }
        //} 

        //public DateTime Date
        //{ 
        //    get => _date; 
        //    set 
        //    {
        //        if(_date == value) return;
        //        _date = value;
        //        OnChanged(nameof(Date));
        //    } 
        //}

    }
}
