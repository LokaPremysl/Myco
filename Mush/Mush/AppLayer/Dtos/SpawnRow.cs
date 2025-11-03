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
        private string _material = string.Empty;
        private DateTime _date = DateTime.Today;

        public string Material
        {
            get => _material;
            set
            {
                { if (_material == value) return;
                    _material = value;
                    OnChanged(nameof(Material));
                }
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date == value) return;
                _date = value;
                OnChanged(nameof(Date));
            }
        }

        public BindingList<BulkRow> Bulks { get; } = new();
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnChanged(string p) => PropertyChanged?.Invoke(this, new(p));
    }
}
