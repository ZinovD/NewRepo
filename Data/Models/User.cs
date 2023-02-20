using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FurnitureStore.Data.Models
{
    public class User: IdentityUser
    {
        public string SurName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
    }
}
