using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Models;

namespace OnBalance.ViewModels.User
{
    public class DashboardViewModel
    {

        public List<Shop> Shops { get; set; }

        public List<Task> Imports { get; set; }

        public List<Task> Exports { get; set; }

    }
}