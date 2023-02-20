using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Models
{
    public class OrderDetail
    {
        [Key]
        public int DetailID { get; set; }
        public int OrderID { get; set; }
        public int FurnitureID { get; set; }
        public int Price { get; set; }
        public virtual Furniture Furniture { get; set; }
        public virtual Order Order { get; set; }
    }
}