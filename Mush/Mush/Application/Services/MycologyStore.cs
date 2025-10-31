using Mush.Application.Dtos;
using Mush.Application.Ports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms.Design;


namespace Mush.src.Infrastructure.Stores
{
    public sealed class MycologyStore : IMycologyStore
    {
        public BindingList<MyceliumRow> Myceliums { get; } = new();
        public MyceliumRow AddMycelium(string mycelium, DateTime date, string origin)
        {
            var row = new MyceliumRow
            { 
            Mycelium = mycelium,
            Date = date,
            Origin = origin
            };
            Myceliums.Add(row);
            return row;
        }

        public SpawnRow AddSpawn(MyceliumRow parent, string material, DateTime date)
        {
            var row = new SpawnRow
            {
                Material = material,
                Date = date,
            };
            parent.Spawns.Add(row);
            return row;
        }

        public BulkRow AddBulk(SpawnRow parent, string material, DateTime date)
        {
            var row = new BulkRow
            {
                Material = material,
                Date = date,
            };
            parent.Bulks.Add(row);
            return row;
        }
    }
}
