using FurnitureStore.Data.Models;
using System.Collections.Generic;

namespace FurnitureStore.ViewModels
{
    public class StockListViewModel
    {
        public IEnumerable<Stock> allStock { get; set; }
    }
}
