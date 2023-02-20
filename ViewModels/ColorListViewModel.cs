using FurnitureStore.Data.Models;
using System.Collections.Generic;

namespace FurnitureStore.ViewModels
{
    public class ColorListViewModel
    {
        public IEnumerable<FurnitureKind> allColor { get; set; }
    }
}