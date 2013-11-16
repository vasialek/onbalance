using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace OnBalance.Domain.Entities
{

    public class Organization// : BaseModel
    {

        public int Id { get; set; }

        public byte StatusId { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public Nullable<DateTime> UpdatedAt { get; set; }

        public OrganizationConfiguration Configuration { get; set; }

    }
}
