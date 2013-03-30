using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnBalance.Models
{
    public class UserCreate
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int OrganizationId { get; set; }

        [Required]
        [Display(Name = "User name (for login)")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public Organization Organization
        {
            get
            {
                return new OrganizationRepository().GetById(OrganizationId);
            }
        }
    }
}
