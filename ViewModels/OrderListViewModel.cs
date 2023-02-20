using FurnitureStore.Data.Models;
using System.Collections.Generic;

namespace FurnitureStore.ViewModels
{
    public class OrderListViewModel
    {
        public IEnumerable<Order> allOrders { get; set; }
    }
}
