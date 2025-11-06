using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mush.AppLayer.Dtos;

namespace Mush.AppLayer.Ports
{
    public interface IMycologyStore
    {
        BindingList<MyceliumRow> Myceliums { get; }

        MyceliumRow AddMycelium(string myselium, DateTime date, string Origin);
        SpawnRow AddSpawn(Guid myceliumId, string material, DateTime date);
        BulkRow AddBulk(Guid spawnId, string material, DateTime date);

        //MyceliumRow? GetMycelium(Guid id);
        //SpawnRow? GetSpawn(Guid id);
        //Task LoadAsync(string path);
        //Task SaveAsync(string path);
    }
}
