using FurnitureStore.Data.Models;
using System.Collections.Generic;

namespace FurnitureStore.ViewModels
{
    public class KindListViewModel
    {
        public IEnumerable<FurnitureKind> allKinds { get; set; }
    }
}
