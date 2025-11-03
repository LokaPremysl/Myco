using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mush.AppLayer.Dtos
{
    public sealed class MyceliumRow : INotifyPropertyChanged
    {
        private string _mycelium = string.Empty;
        private string _origin = string.Empty;
        private DateTime _date = DateTime.Today;

        public string Mycelium
        {
            get => _mycelium;
            set
            {
                if (_mycelium == value) return;
                _mycelium = value;
                OnChanged(nameof(Mycelium));
            }
        }

        public string Origin
        {
            get => _origin;
            set
            {
                if (_origin == value) return;
                _origin = value;
                OnChanged(nameof(Origin));
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

        public BindingList<SpawnRow> Spawns { get; } = new();
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnChanged(string p) => PropertyChanged?.Invoke(this, new(p));
    }
}
