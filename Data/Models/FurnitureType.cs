using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Models
{
    public class FurnitureType
    {
        [Key]
        public int TypeID { get; set; }
        public string Type { get; set; }
        public List<Furniture> Furnitures { get; set; }
    }
}
