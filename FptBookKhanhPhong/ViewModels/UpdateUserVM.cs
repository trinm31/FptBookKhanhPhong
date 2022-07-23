using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FptBookKhanhPhong.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FptBookKhanhPhong.ViewModels
{
    public class UpdateUserVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        [Required] public string Role { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}