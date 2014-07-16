using OnBalance.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public class Task
    {

        /// <summary>
        /// Whether task is import, export, ...
        /// </summary>
        public enum TypeId { No = 0, Import, Export }

        public int Id { get; set; }

        public string Name { get; set; }

        public TypeId Type { get; set; }

        public Status Status { get; set; }

        public int ShopId { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public Task()
        {
            Type = TypeId.No;
            CreatedAt = DateTime.Now;
            Status = Status.Pending;
        }
    }
}
