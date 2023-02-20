using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Models
{
    public class FurnitureKind
    {
        [Key]
        public int KindID { get; set; }
        public string Kind { get; set; }
        public string Photo { get; set; }
        public List<Furniture> Furnitures { get; set; }
    }
}
