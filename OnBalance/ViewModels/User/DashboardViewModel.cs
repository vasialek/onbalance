using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Models;

namespace OnBalance.ViewModels.User
{
    public class DashboardViewModel
    {

        public List<Organization> Shops { get; set; }

        public List<Task> Imports { get; set; }

        public List<Task> Exports { get; set; }

        public DashboardViewModel()
        {
            Shops = new List<Organization>();
            Shops = new OrganizationRepository().Items.ToList(); //.Where(x => x.UserId == User.Identity.Name).ToList();
            Imports = new List<Task>()
            {
                new Task{ Type = Task.TypeId.Import, Status = Status.Pending }
                , new Task{ Type = Task.TypeId.Import, Status = Status.Pending }
            };
            Exports = new List<Task>();
        }

    }
}