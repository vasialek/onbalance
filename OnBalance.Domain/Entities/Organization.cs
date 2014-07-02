using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace OnBalance.Domain.Entities
{

    [Table("vasialek_onbalance_user.organization")]
    public class Organization : BaseModel
    {

        public int Id { get; set; }

        [Column("status_id")]
        public byte StatusId { get; set; }

        [Column("parent_id")]
        public int ParentId { get; set; }

        public string Name { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public Nullable<DateTime> UpdatedAt { get; set; }

        [NotMapped]
        public OrganizationConfiguration Configuration { get; set; }

        [NotMapped]
        public IList<Organization> Children
        {
            get
            {
                return new List<Organization>();
            }
        }

    }
}
