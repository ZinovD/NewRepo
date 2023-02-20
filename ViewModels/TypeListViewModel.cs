using FurnitureStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.ViewModels
{
    public class TypeListViewModel
    {
        public IEnumerable<FurnitureType> allType { get; set; }

    }
}
