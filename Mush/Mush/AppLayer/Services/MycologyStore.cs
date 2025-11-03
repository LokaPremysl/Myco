using Mush.AppLayer.Dtos;
using Mush.AppLayer.Ports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms.Design;


namespace Mush.AppLayer.Services
{
    

    public sealed class MycologyStore : IMycologyStore
    {
        private readonly Dictionary<Guid, MyceliumRow> _mycById = new();
        private readonly Dictionary<Guid, SpawnRow> _spawnById = new();
        private readonly Dictionary<Guid, BulkRow> _bulkById = new();

        

        public BindingList<MyceliumRow> Myceliums { get; } = new();

        public MycologyStore()
        {
            // Pokud už máš nějaká startovní data, tady můžeš zkonstruovat indexy:
            RebuildIndexes();
        }

        private void RebuildIndexes()
        {
            _mycById.Clear(); _spawnById.Clear(); _bulkById.Clear();
            foreach (var m in Myceliums)
            {
                _mycById[m.Id] = m;
                foreach (var s in m.Spawns)
                {
                    _spawnById[s.Id] = s;
                    foreach (var b in s.Bulks)
                        _bulkById[b.Id] = b;
                }
            }
        }

        public MyceliumRow AddMycelium(string mycelium, DateTime date, string origin)
        {
            var row = new MyceliumRow
            {
                Mycelium = mycelium,
                Date = date,
                Origin = origin
            };
            Myceliums.Add(row);
            _mycById[row.Id] = row;
            return row;
        }

        //public SpawnRow AddSpawn(MyceliumRow parent, string material, DateTime date)
        //{
        //    var row = new SpawnRow
        //    {
        //        Material = material,
        //        Date = date,
        //    };
        //    parent.Spawns.Add(row);
        //    return row;
        //}

        //public SpawnRow AddSpawn(Guid myceliumId, string material, DateTime date)
        //{
        //    if (!_mycById.TryGetValue(myceliumId, out var parent))
        //        throw new KeyNotFoundException("Mycelium not found");

        //    var row = new SpawnRow { MyceliumId = myceliumId, Material = material, Date = date};
        //    parent.Spawns.Add(row);
        //    _spawnById[row.Id] = row;
        //    return row;
        //}

        public SpawnRow AddSpawn(Guid myceliumId, string material, DateTime date /*, …*/)
        {
            if (!_mycById.TryGetValue(myceliumId, out var parent))
                throw new KeyNotFoundException("Mycelium not found");

            var row = new SpawnRow
            {
                Id = Guid.NewGuid(),
                MyceliumId = myceliumId,
                Material = material,
                Date = date
                // … doplň volitelné parametry
            };
            parent.Spawns.Add(row);
            _spawnById[row.Id] = row;
            return row;
        }

        //public BulkRow AddBulk(Guid spawnId, string material, DateTime date)
        //{
        //    if (!_spawnById.TryGetValue(spawnId, out var parent))
        //        throw new KeyNotFoundException("Spawn not found");

        //    var row = new BulkRow
        //    {
        //        SpawnId = spawnId,
        //        Material = material,
        //        Date = date
        //    };

        //    parent.Bulks.Add(row);
        //    _bulkById[row.Id] = row;
        //    return row;
        //}

        public BulkRow AddBulk(Guid spawnId, string material, DateTime date /*, …*/)
        {
            if (!_spawnById.TryGetValue(spawnId, out var parent))
                throw new KeyNotFoundException("Spawn not found");

            var row = new BulkRow
            {
                Id = Guid.NewGuid(),
                SpawnId = spawnId,
                Material = material,
                Date = date
                // … doplň volitelné parametry
            };
            parent.Bulks.Add(row);
            _bulkById[row.Id] = row;
            return row;
        }

        public bool RemoveMycelium(Guid id)
        {
            if (!_mycById.TryGetValue(id, out var m)) return false;

            // odstranit indexy dětí
            foreach (var s in m.Spawns)
            {
                foreach (var b in s.Bulks) _bulkById.Remove(b.Id);
                _spawnById.Remove(s.Id);
            }
            _mycById.Remove(id);
            return Myceliums.Remove(m);
        }

        public bool RemoveSpawn(Guid id)
        {
            if (!_spawnById.TryGetValue(id, out var s)) return false;
            if (!_mycById.TryGetValue(s.MyceliumId, out var parent)) return false;

            foreach (var b in s.Bulks) _bulkById.Remove(b.Id);
            _spawnById.Remove(id);
            return parent.Spawns.Remove(s);
        }

        public bool RemoveBulk(Guid id)
        {
            if (!_bulkById.TryGetValue(id, out var b)) return false;
            if (!_spawnById.TryGetValue(b.SpawnId, out var parent)) return false;

            _bulkById.Remove(id);
            return parent.Bulks.Remove(b);
        }
    }
}

