﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mush.src.Application.Dtos;

namespace Mush.Application.Ports
{
    public interface IMycologyStore
    {
        BindingList<MyceliumRow> Myceliums { get; }

        MyceliumRow AddMycelium(string myselium, DateTime date, string Origin);
        SpawnRow AddSpawn(MyceliumRow parent, string material, DateTime date);
        BulkRow AddBulk(SpawnRow parent, string material, DateTime date);

        //Task LoadAsync(string path);
        //Task SaveAsync(string path);
    }
}
