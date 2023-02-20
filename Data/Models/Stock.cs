using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Data.Models
{
    [Index("StockName", IsUnique = true, Name = "Stock_Index")]
    public class Stock
    {
        [Key]
        public int StockID { get; set; }
        public bool IsFavorite { get; set; }
        public string Photo { get; set; }
        public string StockName { get; set; }  
        public double StockSize { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public DateTime StockStartTime { get; set; }
        public DateTime StockEndTime { get; set; }
        public List<Furniture> Furnitures { get; set; }
        
    }

}
