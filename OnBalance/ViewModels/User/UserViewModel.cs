using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnBalance.ViewModels.User
{
    public class UserViewModel
    {

        /// <summary>
        /// Gets/sets username
        /// </summary>
        [Required]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Is approved")]
        public bool IsApproved { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}