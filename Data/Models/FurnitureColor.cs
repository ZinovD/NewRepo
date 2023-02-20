using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Models
{
    public class FurnitureColor
    {
        [Key]
        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public string Color { get; set; }
        public List<Furniture> Furnitures { get; set; }
    }
}