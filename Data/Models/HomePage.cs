using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Data.Models
{
    public class HomePage
    {
        public int HomePageID { get; set; }
        [Required(ErrorMessage = " Не указано название")]
        public string Name { get; set; }
        public bool ImageBlock { get; set; }
        public bool CharacteristicsBlock { get; set; }
        public bool StockBlock { get; set; }
        public bool KindBlock { get; set; }
        public bool OurChoiceBlock { get; set; }
        public string TitleImage_1 { get; set; }
        public string TitleImage_2 { get; set; }
        public string TitleImage_3 { get; set; }
        public string Title_1 { get; set; }
        public string Title_2 { get; set; }
        public string Title_3 { get; set; }
        public string CircleImage_1 { get; set; }
        public string CircleImage_2 { get; set; }
        public string CircleImage_3 { get; set; }
        public string CircleText_1 { get; set; }
        public string CircleText_2 { get; set; }
        public string CircleText_3 { get; set; }
        public bool isFavorite { get; set; }
        public virtual Stock Stock_1 { get; set; }
        public virtual Stock Stock_2 { get; set; }
        public virtual Stock Stock_3 { get; set; }
        

        public List<FurnitureKind> Kind { get; set; }
    }
}
