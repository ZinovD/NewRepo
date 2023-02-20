using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.ViewModels
{
    public class CreateUserViewModel
    {
        public string Email { get; set; }
        public string SurName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
    }
}
