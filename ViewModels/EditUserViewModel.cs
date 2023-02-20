using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FurnitureStore.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string SurName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
