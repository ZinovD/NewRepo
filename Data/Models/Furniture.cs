using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Models
{
    public class Furniture
    {
        public int FurnitureID { get; set; }
        public int TypeID { get; set; }
        public int KindID { get; set; }
        public int ColorID { get; set; }
        public int StockID { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public int Price { get; set; }
        public string Photo1 { get; set; }
        public string Photo2 { get; set; }
        public string Photo3 { get; set; }
        public double StockPrice { get; set; }

        public bool isFavorite { get; set; }
        public string Material { get; set; }
        public string LongDesc { get; set; }

        public virtual FurnitureType FurnitureType { get; set; }
        public virtual FurnitureKind FurnitureKind { get; set; }
        public virtual FurnitureColor FurnitureColor { get; set; }
        public List<Comments> Comments { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
