using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Data.Models
{
    public class Comments
    {
        [Key]
        public int CommentID { get; set; }
        public int FurnitureID { get; set; }
        public string UserName { get; set; }
        public string IdentityUserName { get; set; }
        public DateTime Date { get;set; }
        public string Comment { get; set; }
        public string Avatar { get; set; }
        public string Photo { get; set; }
        public string Photo2 { get; set; }
        public string Photo3 { get; set; }
    }
}
