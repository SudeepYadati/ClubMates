﻿using ClubMates.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clubmates.Web.Models.AccountViewModel
{
    public class UserViewModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public ClubMatesRole Role { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; } = [];

    }
}