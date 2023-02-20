using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Models
{
    public class ShopCartItem
    {
        [Key]
        public int itemID { get; set; }
        public Furniture furniture { get; set; }
        public int FurnitureID { get; set; }
        public int price { get; set; }
        public string ShopCartId { get; set; }
    }
}
