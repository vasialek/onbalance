using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBalance.Domain.Entities
{

    [Table("vasialek_onbalance_user.category")]
    public class Category// : BaseModel
    {
        public int Id { get; set; }

        [Column("status_id")]
        public byte StatusId { get; set; }

        [Column("parent_id")]
        public int ParentId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("category_type_id")]
        public int CategoryTypeId { get; set; }

        //public EntitySet<Product> _Products { get; set; }

        //[NotMapped]
        //protected Category Parent { get; set; }

        //[NotMapped]
        //public string CategoryTypeName { get; set; }

        //protected CategoryType _categoryType = null { get; set; }
        
        //protected CategoryStructure[] _categoryStructure = null { get; set; }


    }
}
