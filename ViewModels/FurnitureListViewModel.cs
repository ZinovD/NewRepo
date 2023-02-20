using FurnitureStore.Data.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.ViewModels
{
    public class FurnitureListViewModel
    {
        public IEnumerable<Furniture> allFurniture { get; set; }
    }
}
